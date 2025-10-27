using System.Collections.Generic;

[System.Serializable]
public class UserAccount
{
    public string Username;
    public List<Achievement> Achievements = new List<Achievement>();
    public SaveData[] Saves = new SaveData[3];

    public UserAccount(string username)
    {
        Username = username;
        for (int i = 0; i < Saves.Length; i++)
            Saves[i] = new SaveData();

        // Initialize achievements from master list
        foreach (var ach in AchievementLib.All)
            Achievements.Add(new Achievement(ach.Id, ach.Name));
    }
}
