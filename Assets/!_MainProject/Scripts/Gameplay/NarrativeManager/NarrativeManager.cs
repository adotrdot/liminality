using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to manage the narration sequences in a stage.
/// </summary>
public class NarrativeManager : MonoBehaviour
{
    #region Fields and properties

    [Header("Narrative Data")]
    [Tooltip("The narrative data for this stage.")]
    public NarrativeData NarrativeData;
    private List<NarrativeLine> m_narrativeLines => NarrativeData.NarrativeLines;
    private string m_choiceA => NarrativeData.PlayerChoiceA;
    private string m_choiceB => NarrativeData.PlayerChoiceB;
    private int m_totalLines => NarrativeData.NarrativeLines.Count;
    private int m_currentLineIndex = 0;

    [Header("Narrative UI")]
    [Tooltip("The narrative canvas UI.")]
    public NarrativeCanvas NarrativeCanvas;
    [HideInInspector] public bool IsNarrativeFinished = false;

    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        // Set this narrative manager in game manager
        GameManager.Instance.SetNarrativeManager(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Play the next line of the narrative.
    /// </summary>
    /// <param name="segmentPosition"></param>
    public void PlayNextNarrativeLine(Vector2 segmentPosition)
    {
        // Cancel if narrative is currently playing
        if (NarrativeCanvas.IsActive)
            return;

        // Play next line if available
        if (m_currentLineIndex < m_totalLines)
        {
            NarrativeLine currentLine = m_narrativeLines[m_currentLineIndex];
            NarrativeCanvas.ShowNarrativeText(currentLine.NarrativeText, currentLine.LineDuration, segmentPosition);
            m_currentLineIndex++;
        }
        else
        {
            IsNarrativeFinished = true;
        }
    }

    /// <summary>
    /// Play the player choice segment of the narrative.
    /// </summary>
    /// <param name="segmentPosition"></param>
    public void PlayChoiceSegment(Vector2 segmentPosition)
    {
        // Cancel if narrative is currently playing
        if (NarrativeCanvas.IsActive)
            return;

        // Show choice texts
        NarrativeCanvas.ShowChoiceTexts(m_choiceA, m_choiceB, segmentPosition);
    }

    /// <summary>
    /// For ending narrative, exhaust the narrative data line by line automatically.
    /// </summary>
    public IEnumerator PlayEndingNarrative()
    {
        foreach (NarrativeLine narrativeLine in m_narrativeLines)
        {
            NarrativeCanvas.gameObject.SetActive(true);
            NarrativeCanvas.ShowNarrativeText(narrativeLine.NarrativeText, narrativeLine.LineDuration, Vector2.zero);
            yield return new WaitUntil(() => !NarrativeCanvas.IsActive);
        }

        GameManager.Instance.EndGame();
    }

    #endregion
}
