[System.Serializable]
public class Achievement
{
    public string Id;
    public string Name;
    public bool IsUnlocked;

    public Achievement(string id, string name, bool unlocked = false)
    {
        Id = id;
        Name = name;
        IsUnlocked = unlocked;
    }
}