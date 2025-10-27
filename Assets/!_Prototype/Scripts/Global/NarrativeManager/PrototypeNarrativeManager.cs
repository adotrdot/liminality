using System.Collections.Generic;
using UnityEngine;

public class PrototypeNarrativeManager : MonoBehaviour
{
    #region Fields and properties

    public PrototypeNarrativeData NarrativeData;
    private List<PrototypeNarrativeLine> m_narrativeLines => NarrativeData.NarrativeLines;
    private string m_choiceA => NarrativeData.PlayerChoiceA;
    private string m_choiceB => NarrativeData.PlayerChoiceB;
    private int m_totalLines => NarrativeData.NarrativeLines.Count;
    private int m_currentLineIndex = 0;

    public PrototypeNarrativeCanvas NarrativeCanvas;
    public bool IsNarrativeFinished = false;

    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        // Set this narrative manager in game manager
        PrototypeGameManager.Instance.SetNarrativeManager(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Public Methods

    public void PlayNextNarrativeLine(Vector2 segmentPosition)
    {
        // Cancel if narrative is currently playing
        if (NarrativeCanvas.IsActive)
            return;

        // Play next line if available
        if (m_currentLineIndex < m_totalLines)
        {
            PrototypeNarrativeLine currentLine = m_narrativeLines[m_currentLineIndex];
            NarrativeCanvas.ShowNarrativeText(currentLine.NarrativeText, currentLine.LineDuration, segmentPosition);
            m_currentLineIndex++;
        }
        else
        {
            IsNarrativeFinished = true;
        }
    }

    public void PlayChoiceSegment(Vector2 segmentPosition)
    {
        // Cancel if narrative is currently playing
        if (NarrativeCanvas.IsActive)
            return;

        // Show choice texts
        NarrativeCanvas.ShowChoiceTexts(m_choiceA, m_choiceB, segmentPosition);
    }

    #endregion
}
