using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton class that manages the overall game state and flow.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Fields and properties

    public static GameManager Instance { get; private set; }

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
    public int CurrentNarrativeDataIndex { get; private set; } = -1;
    public bool IsEnding => CurrentNarrativeDataIndex >= m_totalNarrativeDataCount;

    // Keep track of ending score
    public int EndingScoreA { get; private set; }
    public int EndingScoreB { get; private set; }

    // References to in-game manager components.
    private LevelSpawner m_levelSpawner;
    private NarrativeManager m_narrativeManager;

    // Reference to current active user
    public UserAccount CurrentActiveUser { get; private set; }
    public bool HasActiveUser => CurrentActiveUser != null;

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
        // Initialize user database
        UserDatabase.Instance.Load();

        // Load main menu
        Loader.Instance.LoadMainMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Public methods
    
    public void ResetGameManager()
    {
        CurrentNarrativeDataIndex = -1;
        EndingScoreA = 0;
        EndingScoreB = 0;
    }

    public void NextStage()
    {
        CurrentNarrativeDataIndex++;

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

    public void BackToMainMenu()
    {
        // Reset index, score, and go back to main menu
        ResetGameManager();
        Loader.Instance.LoadMainMenu();
    }

    public void LoadGame(SaveData saveData)
    {
        CurrentNarrativeDataIndex = saveData.NarrativeDataIndex;
        EndingScoreA = saveData.EndingScoreA;
        EndingScoreB = saveData.EndingScoreB;
        Loader.Instance.LoadLevel();
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

    public void HandleSegmentTrigger(Transform collisionRoot, bool isEnteredFromBelow)
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
        EndingScoreA++;
    }

    public void IncrementEndingScoreB()
    {
        EndingScoreB++;
    }

    public void SetActiveUser(UserAccount user)
    {
        CurrentActiveUser = user;
    }

    public void ClearActiveUser()
    {
        CurrentActiveUser = null;
    }

    public void EndGame()
    {
        if (EndingScoreA >= 3)
        {
            // ENDING A
            CurrentActiveUser.Achievements[0].IsUnlocked = true;
        }
        else if (EndingScoreB >= 3)
        {
            // ENDING B
            CurrentActiveUser.Achievements[1].IsUnlocked = true;
        }
        else
        {
            // ENDING C
            CurrentActiveUser.Achievements[2].IsUnlocked = true;
        }

        // Save to database
        UserDatabase.Instance.Save();

        BackToMainMenu();
    }

    #endregion

    #region Private methods

    private void SetNarrativeManagerData()
    {
        if (!IsEnding)
        {
            m_narrativeManager.NarrativeData = NarrativeDataList[CurrentNarrativeDataIndex];
        }
        else if (EndingScoreA >= 3)
        {
            m_narrativeManager.NarrativeData = NarrativeDataEndingA;
        }
        else if (EndingScoreB >= 3)
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
