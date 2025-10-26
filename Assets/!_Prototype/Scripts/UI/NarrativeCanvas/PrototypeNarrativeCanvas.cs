using DG.Tweening;
using TMPro;
using UnityEngine;

public class PrototypeNarrativeCanvas : MonoBehaviour
{
    #region Fields and properties

    public TextMeshProUGUI NarrativeTextUI;

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
    
    public void ShowNarrativeText(string narrativeText, int lineDuration)
    {
        // Kill any running tween to prevent overlap
        m_currentTween?.Kill();

        // Make sure the object is active
        gameObject.SetActive(true);

        // Set the text immediately, but invisible
        NarrativeTextUI.text = narrativeText;
        NarrativeTextUI.alpha = 0f;

        // Fade in, wait, fade out, hide
        m_currentTween = DOTween.Sequence()
            .Append(NarrativeTextUI.DOFade(1f, 0.5f))
            .AppendInterval(lineDuration)
            .Append(NarrativeTextUI.DOFade(0f, 0.5f))
            .OnComplete(() => HideNarrativeText());
    }
    
    public void HideNarrativeText()
    {
        this.gameObject.SetActive(false);
    }

    #endregion
}
