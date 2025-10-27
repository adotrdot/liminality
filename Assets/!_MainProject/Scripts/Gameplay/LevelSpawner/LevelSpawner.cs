using UnityEngine;

/// <summary>
/// A class to manage the spawn of level segment in a game.
/// </summary>
public class LevelSpawner : MonoBehaviour
{
    #region Fields and Properties

    [Header("Level Segment Settings")]

    [Tooltip("Segment object pool to retrieve path segments from.")]
    [SerializeField] private SegmentObjectPool m_segmentObjectPool;

    [Tooltip("Height of the path segment.")]
    [SerializeField] private float m_segmentHeight;

    [Header("Segment Spawn Points")]
    [SerializeField] private GameObject m_segmentSpawnPoint1;
    [SerializeField] private GameObject m_segmentSpawnPoint2;
    private GameObject m_currentSpawnPoint;
    private GameObject m_otherSpawnPoint;
    private int m_segmentIndex = 0;
    private int m_latestSegmentIndex = 0;

    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        // Set this level spawner in game manager
        GameManager.Instance.SetLevelSpawner(this);

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

    /// <summary>
    /// Update the current segment spawn point object.
    /// </summary>
    /// <param name="newCurrentSegment"></param>
    public void UpdateCurrentSegment(GameObject newCurrentSegment)
    {
        m_currentSpawnPoint = newCurrentSegment;
    }

    /// <summary>
    /// Spawn a new segment based on player direction,
    /// and whether the new segment should branch or not.
    /// </summary>
    /// <param name="isEnteredFromBelow"></param>
    /// <param name="isBranching"></param>
    public void SpawnSegment(bool isEnteredFromBelow, bool isBranching = false)
    {
        m_otherSpawnPoint = (m_currentSpawnPoint == m_segmentSpawnPoint1)
                                        ? m_segmentSpawnPoint2 : m_segmentSpawnPoint1;

        // Deactivate segment in other spawn point
        DeactivateSegmentAtPoint(m_otherSpawnPoint);

        if (isEnteredFromBelow)
        {
            // Increment segment index
            if (IsInLatestSegment()) m_latestSegmentIndex++;
            m_segmentIndex++;

            // Place other spawn point above current
            m_otherSpawnPoint.transform.position = new Vector2(
                m_currentSpawnPoint.transform.position.x,
                m_currentSpawnPoint.transform.position.y + m_segmentHeight
            );

            // Spawn straight path segment at other spawn point if not branching,
            // otherwise spawn branching segment ahead.
            if (!isBranching)
            {
                SpawnStraightPathSegment(m_otherSpawnPoint);
            }
            else
            {
                SpawnBranchingPathSegment(m_otherSpawnPoint);
            }
        }
        else
        {
            // Decrement segment index
            m_segmentIndex--;

            // Place other spawn point below current
            m_otherSpawnPoint.transform.position = new Vector2(
                 m_currentSpawnPoint.transform.position.x,
                 m_currentSpawnPoint.transform.position.y - m_segmentHeight
             );

            if (m_segmentIndex > 1)
            {
                // Spawn straight path segment at other spawn point
                SpawnStraightPathSegment(m_otherSpawnPoint);
            }
            else if (m_segmentIndex == 1)
            {
                // Spawn beginning path segment at other spawn point
                SpawnBeginningPathSegment(m_otherSpawnPoint);
            }
        }
    }

    /// <summary>
    /// Get the world position of the current segment spawn point
    /// at which the player is currently in.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetCurrentSegmentWorldPosition()
    {
        if (m_currentSpawnPoint == null)
        {
            Debug.LogWarning("Current spawn point is null. Returning zero vector.");
            return Vector2.zero;
        }

        return (Vector2)m_currentSpawnPoint.transform.position;
    }

    /// <summary>
    /// Check whether player is in the latest segment.
    /// </summary>
    /// <returns></returns>
    public bool IsInLatestSegment()
    {
        return m_segmentIndex == m_latestSegmentIndex;
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

    private void SpawnBranchingPathSegment(GameObject spawnPoint)
    {
        GameObject branchingSegment = m_segmentObjectPool.GetBranchingPathSegment();
        SpawnSegmentAtPoint(branchingSegment, spawnPoint);
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
            m_segmentObjectPool.ReturnPathSegmentToPool(segment);
        }
    }

    #endregion
}