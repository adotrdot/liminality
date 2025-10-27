using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// A class to manage the narrative UI in a stage.
/// </summary>
public class NarrativeCanvas : MonoBehaviour
{
    #region Fields and properties

    [Header("UI Component References")]
    [Tooltip("Reference to the text UI for displaying the narrative line.")]
    public TextMeshProUGUI NarrativeTextUI;

    [Tooltip("Reference to the text UI for displaying player choice A.")]
    public TextMeshProUGUI ChoiceTextA_UI;

    [Tooltip("Reference to the text UI for displaying player choice B.")]
    public TextMeshProUGUI ChoiceTextB_UI;

    [Header("UI Settings")]
    [Tooltip("Offset from the segment position to place the narrative UI.")]
    public Vector2 OffsetToSegmentPosition;

    // Whether narrative is currenty playing a line.
    public bool IsActive => this.gameObject.activeSelf;

    // Holds the current active tween.
    private Tween m_currentTween;

    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        // Initially hide the narrative text
        HideNarrativeUI();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Show narrative text UI with fade in and fade out effects.
    /// </summary>
    /// <param name="narrativeText"></param>
    /// <param name="lineDuration"></param>
    /// <param name="segmentPosition"></param>
    public void ShowNarrativeText(string narrativeText, int lineDuration, Vector2 segmentPosition)
    {
        InitializeNarrativeUI(segmentPosition);

        // Set narrative text
        NarrativeTextUI.text = narrativeText;

        // Begin tween sequence
        m_currentTween = DOTween.Sequence()
            .Append(NarrativeTextUI.DOFade(1f, 0.5f))
            .AppendInterval(lineDuration)
            .Append(NarrativeTextUI.DOFade(0f, 0.5f))
            .OnComplete(() => HideNarrativeUI());
    }

    /// <summary>
    /// Show choice texts UI with fade in effects.
    /// </summary>
    /// <param name="choiceTextA"></param>
    /// <param name="choiceTextB"></param>
    /// <param name="segmentPosition"></param>
    public void ShowChoiceTexts(string choiceTextA, string choiceTextB, Vector2 segmentPosition)
    {
        InitializeNarrativeUI(segmentPosition);

        // Set choice texts
        ChoiceTextA_UI.text = choiceTextA;
        ChoiceTextB_UI.text = choiceTextB;

        // Begin tween sequence
        m_currentTween = DOTween.Sequence()
            .Append(ChoiceTextA_UI.DOFade(1f, 0.5f))
            .Join(ChoiceTextB_UI.DOFade(1f, 0.5f));
    }

    #endregion

    #region Private methods

    // Initialize narrative UI position and state
    private void InitializeNarrativeUI(Vector2 segmentPosition)
    {
        m_currentTween?.Kill();

        Vector2 position = segmentPosition + OffsetToSegmentPosition;
        this.transform.position = position;

        this.gameObject.SetActive(true);
    }

    // Hide narrative UI and reset states
    private void HideNarrativeUI()
    {
        // Deactivate canvas
        this.gameObject.SetActive(false);

        // Clear texts
        NarrativeTextUI.text = string.Empty;
        ChoiceTextA_UI.text = string.Empty;
        ChoiceTextB_UI.text = string.Empty;

        // Reset alphas
        NarrativeTextUI.alpha = 0f;
        ChoiceTextA_UI.alpha = 0f;
        ChoiceTextB_UI.alpha = 0f;
    }

    #endregion
}
