using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton class that manages the overall game state and flow.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Fields and properties

    public static GameManager Instance;

    [Header("Narrative Data")]
    [Tooltip("Holds the narrative data for each stage.")]
    public List<NarrativeData> NarrativeDataList;

    [Tooltip("Holds the narrative data for ending A")]
    public NarrativeData NarrativeDataEndingA;

    [Tooltip("Holds the narrative data for ending B")]
    public NarrativeData NarrativeDataEndingB;

    [Tooltip("Holds the narrative data for ending C")]
    public NarrativeData NarrativeDataEndingC;

    // Variables to keep track of current narrative.
    private int m_totalNarrativeDataCount => NarrativeDataList.Count;
    private int m_currentNarrativeDataIndex = 3;
    public bool IsEnding => m_currentNarrativeDataIndex >= m_totalNarrativeDataCount;

    // References to in-game manager components.
    private LevelSpawner m_levelSpawner;
    private NarrativeManager m_narrativeManager;

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
        m_currentNarrativeDataIndex++;
        Debug.Log("Current narrative data index: " + m_currentNarrativeDataIndex + "| IsEnding: " + IsEnding);

        // Load level if narrative data is not exhausted,
        // otherwise load ending
        if (!IsEnding)
        {
            Loader.Instance.LoadLevel();
        }
        else
        {
            Loader.Instance.LoadEnding();
        }
    }

    public void SetLevelSpawner(LevelSpawner levelSpawner)
    {
        m_levelSpawner = levelSpawner;
    }

    public void SetNarrativeManager(NarrativeManager narrativeManager)
    {
        m_narrativeManager = narrativeManager;
        SetNarrativeManagerData();

        // Immediately play narrative if it's ending
        if (IsEnding) StartCoroutine(m_narrativeManager.PlayEndingNarrative());
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

    public void EndGame()
    {
        Debug.Log("END GAME");
    }

    #endregion

    #region Private methods

    private void SetNarrativeManagerData()
    {
        if (!IsEnding)
        {
            m_narrativeManager.NarrativeData = NarrativeDataList[m_currentNarrativeDataIndex];
        }
        else if (m_endingScoreA >= 3)
        {
            m_narrativeManager.NarrativeData = NarrativeDataEndingA;
        }
        else if (m_endingScoreB >= 3)
        {
            m_narrativeManager.NarrativeData = NarrativeDataEndingB;
        }
        else
        {
            m_narrativeManager.NarrativeData = NarrativeDataEndingC;
        }
        
    }

    #endregion
}
