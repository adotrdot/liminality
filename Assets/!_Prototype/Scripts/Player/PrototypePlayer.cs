using UnityEngine;

public class PrototypePlayer : MonoBehaviour
{
    #region Fields and Properties

    [Header("Movement Settings")]
    [Tooltip("The Rigidbody component attached to the player.")]
    public Rigidbody2D RB;

    [Tooltip("The force applied for player movement.")]
    public float MovementForce = 10.0f;

    [Tooltip("The maximum movement speed of the player.")]
    public float MaxMovementSpeed = 5.0f;

    #endregion

    #region Monobehaviour Methods

    // Reset is called when the script is first added or the Reset option is selected in the Inspector
    void Reset()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Apply force in movement direction
        RB.AddForce(PrototypeInputManager.Instance.Movement * MovementForce);

        // Clamp velocity to max movement speed
        RB.velocity = Vector2.ClampMagnitude(RB.velocity, MaxMovementSpeed);
    }
        
    #endregion
}
