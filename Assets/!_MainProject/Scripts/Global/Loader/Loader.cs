using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Loader class for managing scene loading with animations.
/// </summary>
public class Loader : MonoBehaviour
{
    #region Fields and Properties
    // Singleton instance
    public static Loader Instance { get; private set; }

    [Header("Loader Components")]
    [SerializeField] private Animator m_LoaderAnimator;
    [SerializeField] private UnityEngine.UI.Slider m_LoaderSlider;

    [Header("Scene Names")]
    [SerializeField] private string m_levelSceneName;
    [SerializeField] private string m_endingSceneName;
    [SerializeField] private string m_mainMenuScenename;

    #endregion

    #region Monobehaviour Methods

    private void Awake()
    {
        // Ensure only one instance of Loader exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Public Methods

    public void LoadMainMenu()
    {
        m_LoaderAnimator.SetTrigger("FadeInTrigger");
        StartCoroutine(LoadScene(m_mainMenuScenename));
    }

    /// <summary>
    /// Initiates loading level with a fade-in animation.
    /// </summary>
    public void LoadLevel()
    {
        m_LoaderAnimator.SetTrigger("FadeInTrigger");
        StartCoroutine(LoadScene(m_levelSceneName));
    }

    /// <summary>
    /// Initiates loading the ending level with a fade-in animation.
    /// </summary>
    public void LoadEnding()
    {
        m_LoaderAnimator.SetTrigger("FadeInTrigger");
        StartCoroutine(LoadScene(m_endingSceneName));
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Gets the current state of the loader animator.
    /// </summary>
    /// <returns></returns>
    private AnimatorStateInfo GetLoaderAnimatorStateInfo()
    {
        return m_LoaderAnimator.GetCurrentAnimatorStateInfo(0);
    }

    /// <summary>
    /// Loads a scene asynchronously with a fade-in and fade-out animation.
    /// The scene will be loaded additively.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    /// <returns></returns>
    private IEnumerator LoadScene(string sceneName)
    {
        // Reset slider value
        m_LoaderSlider.value = 0;

        // Wait until fade in animation is finished
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = GetLoaderAnimatorStateInfo();
            return state.IsName("LoaderFadeIn") && state.normalizedTime >= 1.0f;
        });

        // Fade in and show loading screen
        yield return StartCoroutine(Fader.Instance.FadeIn());

        // Set loader scene as active scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("0_Loader"));

        // Unload all other scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != "0_Loader")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // Play loop animation
        m_LoaderAnimator.SetTrigger("LoopTrigger");

        // Load scene additively and async
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            // Update loader bar
            m_LoaderSlider.value = asyncOperation.progress + 1.0f;

            // Check if the loading is almost complete
            if (asyncOperation.progress >= 0.9f)
            {
                // Play loading canvas fadeout animation
                m_LoaderAnimator.SetTrigger("FadeOutTrigger");

                // Fade out from loading screen
                yield return StartCoroutine(Fader.Instance.FadeOut());

                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Activate the loaded scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        // Pause physics and gameplay for a bit
        Time.timeScale = 0f;

        // Fade in to the loaded scene
        yield return StartCoroutine(Fader.Instance.FadeIn());

        // Resume physics and gameplay
        Time.timeScale = 1f;
    }

    #endregion
}