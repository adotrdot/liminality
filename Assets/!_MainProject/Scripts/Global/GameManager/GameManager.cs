using UnityEngine;

/// <summary>
/// GameManager is a singleton class that manages the overall game state and operations.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Fields and Properties

    // Singleton instance
    public static GameManager Instance { get; private set; }

    #endregion

    #region Monobehaviour Methods

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
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

    private void Start()
    {
        // Center lock the cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Load first level
        Loader.Instance.LoadFirstLevel();
    }

    private void Update()
    {
        
    }

    #endregion

    #region Public Methods

    

    #endregion
}