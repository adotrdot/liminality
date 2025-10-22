using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    #region Fields and Properties

    // Singleton instance
    public static Fader Instance { get; private set; }

    [Tooltip("The image component used for fading.")]
    public RawImage FadeImage;

    [Tooltip("The fader text component to display additional text after fading.")]
    public TextMeshProUGUI FaderText;

    [Tooltip("The duration of the fade effect in seconds.")]
    public float FadeDuration = 1.0f;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        // Ensure the image is fully transparent at start
        Color c = FadeImage.color;
        c.a = 0f;
        FadeImage.color = c;

        // Ensure the fader text is fully transparent at start
        Color cText = FaderText.color;
        cText.a = 0f;
        FaderText.color = cText;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Fades from black to transparent (Fade In)
    /// </summary>
    public IEnumerator FadeIn()
    {
        // Fade text to transparent
        Tween fadeTextTween = FaderText
            .DOFade(0f, FadeDuration)
            .SetUpdate(true);;
        yield return fadeTextTween.WaitForCompletion();

        // Fade overlay to transparent
        Tween fadeTween = FadeImage
            .DOFade(0f, FadeDuration)
            .SetUpdate(true);
        yield return fadeTween.WaitForCompletion();
    }

    /// <summary>
    /// Fades from transparent to black (Fade Out)
    /// </summary>
    public IEnumerator FadeOut()
    {
        // Ensure text is transparent
        Color cText = FaderText.color;
        cText.a = 0f;
        FaderText.color = cText;

        // Ensure the image starts transparent black
        FadeImage.color = new Color(0f, 0f, 0f, 0f);
        Tween fadeTween = FadeImage
            .DOFade(1f, FadeDuration)
            .SetUpdate(true);
        yield return fadeTween.WaitForCompletion();
    }

    /// <summary>
    /// Fades from transparent to white (Fade Out White)
    /// </summary>
    public IEnumerator FadeOutToWhite()
    {
        // Ensure the image starts transparent white
        FadeImage.color = new Color(1f, 1f, 1f, 0f);
        Tween fadeTween = FadeImage
            .DOFade(1f, FadeDuration)
            .SetUpdate(true);
        yield return fadeTween.WaitForCompletion();
    }

    public IEnumerator FadeWithText(string textToDisplay)
    {
        // Fade to black first
        yield return StartCoroutine(FadeOut());

        // Fade good ending text
        FaderText.text = textToDisplay;
        Tween fadeTextTween = FaderText
            .DOFade(1f, FadeDuration)
            .SetUpdate(true);
        yield return fadeTextTween.WaitForCompletion();
    }

    #endregion
}