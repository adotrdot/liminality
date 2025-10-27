using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserDatabase : MonoBehaviour
{
    #region Fields and properties

    public static UserDatabase Instance { get; private set; }

    // Save file path
    private const string FILE_PATH = "UserDatabase.dat";

    // Stores user accounts
    [HideInInspector] public List<UserAccount> Users = new List<UserAccount>();

    [Tooltip("Max numbers of users")]
    public int MaxUserCount = 2;

    // Reference to the global achievement list (optional: inject from GameManager)
    private List<Achievement> MasterAchievements => AchievementLib.All;

    #endregion

    #region Monobehaviour methods

    void Awake()
    {
        // Initialize singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Save and Load System

    public void Save()
    {
        string path = Path.Combine(Application.persistentDataPath, FILE_PATH);
        try
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                writer.Write(Users.Count);
                foreach (var user in Users)
                {
                    // Username
                    writer.Write(user.Username);

                    // Achievements
                    writer.Write(user.Achievements.Count);
                    foreach (var ach in user.Achievements)
                    {
                        writer.Write(ach.Id);
                        writer.Write(ach.Name);
                        writer.Write(ach.IsUnlocked);
                    }

                    // Saves
                    writer.Write(user.Saves.Length);
                    foreach (var save in user.Saves)
                    {
                        writer.Write(save.NarrativeDataIndex);
                        writer.Write(save.EndingScoreA);
                        writer.Write(save.EndingScoreB);
                        writer.Write(save.SaveTime.ToBinary());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save user database: {ex.Message}");
        }
    }

    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, FILE_PATH);
        if (!File.Exists(path))
            return;

        try
        {
            Users.Clear();
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                int userCount = reader.ReadInt32();
                for (int i = 0; i < userCount; i++)
                {
                    string username = reader.ReadString();
                    UserAccount account = new UserAccount(username);

                    // Read achievements
                    int achCount = reader.ReadInt32();
                    account.Achievements.Clear();
                    for (int a = 0; a < achCount; a++)
                    {
                        string id = reader.ReadString();
                        string name = reader.ReadString();
                        bool unlocked = reader.ReadBoolean();
                        account.Achievements.Add(new Achievement(id, name, unlocked));
                    }

                    // Read saves
                    int saveCount = reader.ReadInt32();
                    for (int s = 0; s < saveCount; s++)
                    {
                        account.Saves[s].NarrativeDataIndex = reader.ReadInt32();
                        account.Saves[s].EndingScoreA = reader.ReadInt32();
                        account.Saves[s].EndingScoreB = reader.ReadInt32();
                        account.Saves[s].SaveTime = DateTime.FromBinary(reader.ReadInt64());
                    }

                    // ✅ Ensure achievement list matches master (auto add new ones)
                    SyncWithMasterAchievements(account);

                    Users.Add(account);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load user database: {ex.Message}");
        }
    }

    #endregion

    #region User Management

    public void AddUser(string username)
    {
        if (Users.Count >= MaxUserCount)
        {
            Debug.LogWarning("Maximum number of users reached!");
            return;
        }

        // Create a new account with full achievement list
        UserAccount newUser = new UserAccount(username);
        SyncWithMasterAchievements(newUser);

        Users.Add(newUser);
        Save();
    }

    public void DeleteUser(string username)
    {
        Users.RemoveAll(u => u.Username == username);
        Save();
    }

    public void DeleteUser(int index)
    {
        Users.RemoveAt(index);
        Save();
    }

    #endregion

    #region Achivement Management

    private void SyncWithMasterAchievements(UserAccount account)
    {
        foreach (var master in MasterAchievements)
        {
            bool exists = account.Achievements.Exists(a => a.Id == master.Id);
            if (!exists)
            {
                account.Achievements.Add(new Achievement(master.Id, master.Name));
            }
        }
    }

    #endregion
}