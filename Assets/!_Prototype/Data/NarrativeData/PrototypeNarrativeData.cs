using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrototypeNarrativeData", menuName = "Scriptable Objects/PrototypeNarrative Data")]
public class PrototypeNarrativeData : ScriptableObject
{
    [Tooltip("List of narrative lines for a segment.")]
    public List<PrototypeNarrativeLine> NarrativeLines;

    [Tooltip("Text to display for branching path A.")]
    [TextArea(2, 5)]
    public string PlayerChoiceA;

    [Tooltip("Text to display for branching path B.")]
    [TextArea(2, 5)]
    public string PlayerChoiceB;
}
