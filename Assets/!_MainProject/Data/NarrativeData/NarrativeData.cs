using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject that holds narrative data including lines and player choices.
/// </summary>
[CreateAssetMenu(fileName = "NarrativeData", menuName = "Scriptable Objects/Narrative Data")]
public class NarrativeData : ScriptableObject
{
    [Tooltip("List of narrative lines for a segment.")]
    public List<NarrativeLine> NarrativeLines;

    [Tooltip("Text to display for branching path A.")]
    [TextArea(2, 5)]
    public string PlayerChoiceA;

    [Tooltip("Text to display for branching path B.")]
    [TextArea(2, 5)]
    public string PlayerChoiceB;
}
