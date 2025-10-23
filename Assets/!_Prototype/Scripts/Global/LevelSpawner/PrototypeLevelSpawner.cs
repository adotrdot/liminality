using Unity.VisualScripting;
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
    [SerializeField] private GameObject m_currentSegmentSpawnPoint;
    [SerializeField] private GameObject m_nextSegmentSpawnPoint;
    [SerializeField] private GameObject m_prevSegmentSpawnPoint;
    
    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        // Place segment spawn points at their initial positions
        m_currentSegmentSpawnPoint.transform.position = Vector2.zero;
        m_nextSegmentSpawnPoint.transform.position = new Vector2(0, m_segmentHeight);
        m_prevSegmentSpawnPoint.transform.position = new Vector2(0, -m_segmentHeight);

        // Spawn initial segments
        SpawnBeginningPathSegment(m_currentSegmentSpawnPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    #endregion
}
