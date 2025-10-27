using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputManager is responsible for managing input actions in the game.
/// </summary>
public class InputManager : MonoBehaviour
{
    #region Fields and Properties

    // Singleton instance
    public static InputManager Instance { get; private set; }

    // InputAction variables
    [HideInInspector] public InputAction MoveInput;
    [HideInInspector] public InputAction PauseAction;

    // Movement property
    [HideInInspector] public Vector2 Movement => MoveInput.ReadValue<Vector2>().normalized;

    #endregion

    #region Monobehaviour Methods

    private void Awake()
    {
        // Ensure only one instance of InputManager exists
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
        // Initialize all inputs
        MoveInput = InputSystem.actions.FindAction("Move");
        PauseAction = InputSystem.actions.FindAction("Pause");
    }

    #endregion
}