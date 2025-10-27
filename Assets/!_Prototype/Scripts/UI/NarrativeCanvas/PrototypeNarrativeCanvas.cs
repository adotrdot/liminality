using DG.Tweening;
using TMPro;
using UnityEngine;

public class PrototypeNarrativeCanvas : MonoBehaviour
{
    #region Fields and properties

    public TextMeshProUGUI NarrativeTextUI;

    public TextMeshProUGUI ChoiceTextA_UI;
    public TextMeshProUGUI ChoiceTextB_UI;

    public Vector2 OffsetToSegmentPosition;

    public bool IsActive => this.gameObject.activeSelf;

    private Tween m_currentTween;

    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        // Initially hide the narrative text
        HideNarrativeText();
    }

    #endregion

    #region Public methods

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
            .OnComplete(() => HideNarrativeText());
    }
    
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

    private void InitializeNarrativeUI(Vector2 segmentPosition)
    {
        m_currentTween?.Kill();

        Vector2 position = segmentPosition + OffsetToSegmentPosition;
        this.transform.position = position;

        this.gameObject.SetActive(true);
    }

    private void HideNarrativeText()
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
