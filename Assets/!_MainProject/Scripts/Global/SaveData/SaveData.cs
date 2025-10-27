using System;

[System.Serializable]
public class SaveData
{
    public bool IsUsed;
    public int NarrativeDataIndex;
    public int EndingScoreA;
    public int EndingScoreB;
    public DateTime SaveTime;

    public SaveData()
    {
        IsUsed = false;
        NarrativeDataIndex = 0;
        EndingScoreA = 0;
        EndingScoreB = 0;
        SaveTime = DateTime.MinValue;
    }
}
