using UnityEngine;

/// <summary>
/// Represents a single line of narrative text along with its display duration.
/// </summary>
[System.Serializable]
public class NarrativeLine
{
    [Tooltip("Narrative text to display for this line.")]
    [TextArea(3, 10)]
    public string NarrativeText;

    [Tooltip("Duration to display this line (in seconds).")]
    public int LineDuration;
}
