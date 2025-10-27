using UnityEngine;

public class PrototypeGameManager : MonoBehaviour
{
    #region Fields and properties

    public static PrototypeGameManager Instance;

    private PrototypeLevelSpawner m_levelSpawner;
    private PrototypeNarrativeManager m_narrativeManager;

    #endregion

    #region Monobehaviour Methods

    void Awake()
    {
        // Initialize singleton
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

    // Start is called before the first frame update
    void Start()
    {
        InitializeGameManager();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Public methods

    public void InitializeGameManager()
    {
        // Get references to other managers in this stage/scene
        m_levelSpawner = FindObjectOfType<PrototypeLevelSpawner>();
        m_narrativeManager = FindObjectOfType<PrototypeNarrativeManager>();
    }
    
    public void HandleSegmenTrigger(Transform collisionRoot, bool isEnteredFromBelow)
    {
        // Update current segment in level spawner to current segment of which its trigger
        // collided with player
        m_levelSpawner.UpdateCurrentSegment(collisionRoot.gameObject);

        // Play next narrative line only if player is
        // - in the latest segment; and
        // - entering from below
        if (isEnteredFromBelow && m_levelSpawner.IsInLatestSegment())
        {
            m_narrativeManager.PlayNextNarrativeLine(
                m_levelSpawner.GetCurrentSegmentWorldPosition()
            );
        }

        // Spawn segment using level spawner
        m_levelSpawner.SpawnSegment(isEnteredFromBelow);
    }

    #endregion
}
