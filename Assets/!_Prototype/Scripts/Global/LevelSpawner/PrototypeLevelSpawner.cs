using UnityEngine;

public class PrototypeLevelSpawner : MonoBehaviour
{
    #region Fields and Properties

    [Header("Level Segment Settings")]

    [Tooltip("Segment object pool to retrieve path segments from.")]
    [SerializeField] private PrototypeSegmentObjectPool m_segmentObjectPool;

    [Tooltip("Height of the path segment.")]
    [SerializeField] private float m_segmentHeight;

    [Header("Segment Spawn Points")]
    [SerializeField] private GameObject m_segmentSpawnPoint1;
    [SerializeField] private GameObject m_segmentSpawnPoint2;
    
    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        // Place segment spawn points at their initial positions
        m_segmentSpawnPoint1.transform.position = Vector2.zero;
        m_segmentSpawnPoint2.transform.position = new Vector2(0, m_segmentHeight);

        // Spawn initial segments
        SpawnBeginningPathSegment(m_segmentSpawnPoint1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Public Methods

    public void NextTriggerHandle(Transform collisionRoot, bool isEnteredFromBelow)
    {
        GameObject currentSpawnPoint = collisionRoot.gameObject;
        GameObject otherSpawnPoint = (currentSpawnPoint == m_segmentSpawnPoint1)
                                        ? m_segmentSpawnPoint2 : m_segmentSpawnPoint1;

        if (isEnteredFromBelow)
        {
            // Place other spawn point above current
            otherSpawnPoint.transform.position = new Vector2(
                currentSpawnPoint.transform.position.x,
                currentSpawnPoint.transform.position.y + m_segmentHeight
            );

            // Spawn straight path segment at other spawn point
            SpawnStraightPathSegment(otherSpawnPoint);
        }
        else
        {
            // If entered from above, deactivate segment in other spawn point
            // and move them back to object pool
            DeactivateSegmentAtPoint(otherSpawnPoint);
        }
    }
    
    public void PrevTriggerHandle(Transform collisionRoot, bool isEnteredFromBelow)
    {
        GameObject currentSpawnPoint = collisionRoot.gameObject;
        GameObject otherSpawnPoint = (currentSpawnPoint == m_segmentSpawnPoint1)
                                        ? m_segmentSpawnPoint2 : m_segmentSpawnPoint1;

        if (isEnteredFromBelow)
        {
            // Deactivate segment in other spawn point
            // and move them back to object pool
            DeactivateSegmentAtPoint(otherSpawnPoint);
        }
        else
        {
            // Place other spawn point below current
            otherSpawnPoint.transform.position = new Vector2(
                currentSpawnPoint.transform.position.x,
                currentSpawnPoint.transform.position.y - m_segmentHeight
            );

            // Spawn straight path segment at other spawn point
            SpawnStraightPathSegment(otherSpawnPoint);
        }
    }

    #endregion

    #region Private & Protected Methods

    private void SpawnBeginningPathSegment(GameObject spawnPoint)
    {
        GameObject beginningSegment = m_segmentObjectPool.GetBeginningPathSegment();
        SpawnSegmentAtPoint(beginningSegment, spawnPoint);
    }

    private void SpawnStraightPathSegment(GameObject spawnPoint)
    {
        GameObject straightSegment = m_segmentObjectPool.GetStraightPathSegment();
        SpawnSegmentAtPoint(straightSegment, spawnPoint);
    }

    private void SpawnSegmentAtPoint(GameObject segment, GameObject spawnPoint)
    {
        if (segment != null && spawnPoint != null)
        {
            segment.transform.SetParent(spawnPoint.transform);
            segment.transform.localPosition = Vector2.zero;
            segment.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Segment or Spawn Point is null. Cannot spawn segment.");
        }
    }
    
    private void DeactivateSegmentAtPoint(GameObject spawnPoint)
    {
        if (spawnPoint.transform.childCount > 0)
        {
            GameObject segment = spawnPoint.transform.GetChild(0).gameObject;
            segment.SetActive(false);
            m_segmentObjectPool.ReturnPathSegment(segment);
        }
    }

    #endregion
}
