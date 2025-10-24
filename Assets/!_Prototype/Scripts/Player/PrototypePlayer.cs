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

    [Header("Connection with Level Spawner")]
    [Tooltip("Reference to the level spawner in the scene.")]
    [SerializeField] private PrototypeLevelSpawner m_levelSpawner;

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
        string tag = collision.gameObject.tag;
        switch (tag)
        {
            case "NextTrigger":
                m_levelSpawner.NextTriggerHandle(
                    collision.transform.parent.parent,
                    IsTriggerEnteredFromBelow(collision)
                );
                break;
            case "PrevTrigger":
                m_levelSpawner.PrevTriggerHandle(
                    collision.transform.parent.parent,
                    IsTriggerEnteredFromBelow(collision)
                );
                break;
            default:
                Debug.LogWarning("Hit unknown trigger: " + collision.gameObject.name);
                break;
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
