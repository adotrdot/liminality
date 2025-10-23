using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// DefaultSceneLoader is a utility class that automatically loads a specified default scene when entering play mode.
/// </summary>
[InitializeOnLoad]
public static class DefaultSceneLoader
{
    private const string PrefKey = "PlayDefaultSceneEnabled";
    private const string DefaultScenePath = "Assets/!_MainProject/Scenes/0_Loader.unity";

    static DefaultSceneLoader()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (!IsEnabled)
            return;

        if (state == PlayModeStateChange.ExitingEditMode)
        {
            if (EditorSceneManager.GetActiveScene().path != DefaultScenePath)
            {
                EditorSceneManager.OpenScene(DefaultScenePath);
            }
        }
    }

    public static bool IsEnabled
    {
        get => EditorPrefs.GetBool(PrefKey, true); // Default: enabled
        set => EditorPrefs.SetBool(PrefKey, value);
    }

    // Menu item with checkmark
    [MenuItem("Tools/Play Default Scene %#d")] // Ctrl+Shift+D toggles
    private static void TogglePlayDefaultScene()
    {
        IsEnabled = !IsEnabled;
    }

    // Show checkmark in menu
    [MenuItem("Tools/Play Default Scene %#d", true)]
    private static bool TogglePlayDefaultSceneValidate()
    {
        Menu.SetChecked("Tools/Play Default Scene %#d", IsEnabled);
        return true;
    }
}