using UnityEngine;

public class PrototypePlayer : MonoBehaviour
{
    #region Fields and Properties

    [Header("Movement Settings")]
    [Tooltip("The Rigidbody component attached to the player.")]
    [SerializeField] private Rigidbody2D m_RB;

    [Tooltip("The force applied for player movement.")]
    [SerializeField] private float m_movementForce = 20.0f;

    [Tooltip("The maximum movement speed of the player.")]
    [SerializeField] private float m_maxMovementSpeed = 10.0f;

    #endregion

    #region Monobehaviour Methods

    // Reset is called when the script is first added or the Reset option is selected in the Inspector
    void Reset()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Apply force in movement direction
        m_RB.AddForce(PrototypeInputManager.Instance.Movement * m_movementForce);

        // Clamp velocity to max movement speed
        m_RB.velocity = Vector2.ClampMagnitude(m_RB.velocity, m_maxMovementSpeed);
    }

    #endregion

    #region Private & Protected Methods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SegmentTrigger"))
        {
            PrototypeGameManager.Instance.HandleSegmenTrigger(
                collision.transform.parent.parent,
                IsTriggerEnteredFromBelow(collision)
            );
        }
        else
        {
            Debug.LogWarning("Hit unknown trigger: " + collision.gameObject.name);
        }
    }

    private bool IsTriggerEnteredFromBelow(Collider2D triggerCollider)
    {
        float playerY = transform.position.y;
        float triggerY = triggerCollider.transform.position.y;
        return playerY < triggerY;
    }

    #endregion
}
