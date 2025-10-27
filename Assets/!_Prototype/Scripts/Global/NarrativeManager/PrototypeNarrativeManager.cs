using System.Collections.Generic;
using UnityEngine;

public class PrototypeNarrativeManager : MonoBehaviour
{
    #region Fields and properties

    public PrototypeNarrativeData NarrativeData;
    private List<PrototypeNarrativeLine> m_narrativeLines => NarrativeData.NarrativeLines;
    private int m_totalLines => NarrativeData.NarrativeLines.Count;
    private int m_currentLineIndex = 0;

    public PrototypeNarrativeCanvas NarrativeCanvas;
    [HideInInspector] public bool IsNarrativeFinished = false;

    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    #endregion

    #region Public Methods
    
    public void PlayNextNarrativeLine(Vector2 segmentPosition)
    {
        // Cancel if narrative is currently playing or finished
        if (NarrativeCanvas.IsActive || IsNarrativeFinished)
            return;

        // Play next line if available, else mark narrative as finished
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

    #endregion
}
