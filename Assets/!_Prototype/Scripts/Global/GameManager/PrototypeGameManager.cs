using System.Collections.Generic;
using UnityEngine;

public class PrototypeGameManager : MonoBehaviour
{
    #region Fields and properties

    public static PrototypeGameManager Instance;

    public List<PrototypeNarrativeData> NarrativeDataList;
    public int TotalNarrativeDataCount => NarrativeDataList.Count;
    [HideInInspector] int CurrentNarrativeDataIndex = -1;

    private PrototypeLevelSpawner m_levelSpawner;
    private PrototypeNarrativeManager m_narrativeManager;

    // Keep track of ending score
    private int m_endingScoreA = 0;
    private int m_endingScoreB = 0;

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
        // Center lock the cursor
        Cursor.lockState = CursorLockMode.Locked;

        NextStage();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Public methods

    public void NextStage()
    {
        CurrentNarrativeDataIndex++;
        Loader.Instance.LoadLevel();
    }

    public void SetLevelSpawner(PrototypeLevelSpawner levelSpawner)
    {
        m_levelSpawner = levelSpawner;
    }

    public void SetNarrativeManager(PrototypeNarrativeManager narrativeManager)
    {
        m_narrativeManager = narrativeManager;
        SetNarrativeManagerData();
    }

    public void HandleSegmenTrigger(Transform collisionRoot, bool isEnteredFromBelow)
    {
        // Update current segment in level spawner to current segment of which its trigger
        // collided with player
        m_levelSpawner.UpdateCurrentSegment(collisionRoot.parent.gameObject);

        // If end segment is reached, play choice segment
        // and return immediately
        if (collisionRoot.gameObject.CompareTag("BranchingPathSegment"))
        {
            m_narrativeManager.PlayChoiceSegment(
                m_levelSpawner.GetCurrentSegmentWorldPosition()
            );
            return;
        }

        // Play next narrative line only if player is
        // - in the latest segment; and
        // - entering from below
        if (isEnteredFromBelow && m_levelSpawner.IsInLatestSegment())
        {
            if (!m_narrativeManager.IsNarrativeFinished)
            {
                m_narrativeManager.PlayNextNarrativeLine(
                    m_levelSpawner.GetCurrentSegmentWorldPosition()
                );
            }
        }

        // Spawn segment using level spawner
        m_levelSpawner.SpawnSegment(isEnteredFromBelow, m_narrativeManager.IsNarrativeFinished);
    }

    public void IncrementEndingScoreA()
    {
        m_endingScoreA++;
    }

    public void IncrementEndingScoreB()
    {
        m_endingScoreB++;
    }

    public int GetEndingScoreA()
    {
        return m_endingScoreA;
    }

    public int GetEndingScoreB()
    {
        return m_endingScoreB;
    }

    #endregion

    #region Private methods
    
    private void SetNarrativeManagerData()
    {
        m_narrativeManager.NarrativeData = NarrativeDataList[CurrentNarrativeDataIndex];
    }

    #endregion
}
