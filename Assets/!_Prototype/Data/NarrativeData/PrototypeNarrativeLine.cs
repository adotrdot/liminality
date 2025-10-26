using UnityEngine;

[System.Serializable]
public class PrototypeNarrativeLine
{
    [Tooltip("Narrative text to display for this line.")]
    [TextArea(2, 5)]
    public string NarrativeText;

    [Tooltip("Duration to display this line (in seconds).")]
    public int LineDuration;
}
