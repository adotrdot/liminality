using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    #region Fields and properties

    public SaveSystem SaveSystem;
    public List<GameObject> SaveSlotBoxes;
    public GameObject EmptySaveContainerPrefab;
    public GameObject FilledSaveContainerPrefab;

    [SerializeField] private Button m_closeButton;
    [SerializeField] private Button m_mainMenuButton;

    private List<SaveData> m_mySaveData;

    #endregion

    #region Monobehaviour methods

    // Start is called before the first frame update
    void Start()
    {
        // Initally hide this
        HideMenu();

        // Connect listeners
        m_closeButton.onClick.AddListener(HideMenu);
        m_mainMenuButton.onClick.AddListener(HideAndBackToMainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    public void ShowMenu()
    {
        RefreshList();
        this.gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void HideAndBackToMainMenu()
    {
        HideMenu();
        GameManager.Instance.BackToMainMenu();
    }

    public void RefreshList()
    {
        ClearList();

        GetMySaveData();

        // Instantiate based on saved count
        int saveCount = m_mySaveData.Count;
        for (int i = 0; i < saveCount; i++)
        {
            int j = i;
            if (m_mySaveData[j].SaveTime == DateTime.MinValue)
            {
                // Savetime min value means that this save slot is empty
                EmptySaveSlotContainer saveSlotContainer = Instantiate(EmptySaveContainerPrefab, SaveSlotBoxes[j].transform)
                                            .GetComponent<EmptySaveSlotContainer>();
                saveSlotContainer.SetSaveInfo(j);
                saveSlotContainer.OnSave = () =>
                {
                    SaveSystem.SaveGame(j);
                    RefreshList();
                };
            }
            else
            {
                // Filled save
                SaveSlotContainer saveSlotContainer = Instantiate(FilledSaveContainerPrefab, SaveSlotBoxes[j].transform)
                                                        .GetComponent<SaveSlotContainer>();
                saveSlotContainer.SetSaveInfo(j, m_mySaveData[j]);
                saveSlotContainer.OnSave = () =>
                {
                    SaveSystem.SaveGame(j);
                    RefreshList();
                };
                saveSlotContainer.OnLoad = () =>
                {
                    HideMenu();
                    GameManager.Instance.LoadGame(m_mySaveData[j]);
                };
            }
        }
    }

    public void ClearList()
    {
        foreach (GameObject saveSlot in SaveSlotBoxes)
        {
            if (saveSlot.transform.childCount > 0)
                Destroy(saveSlot.transform.GetChild(0).gameObject);
        }
    }
    
    private void GetMySaveData()
    {
        m_mySaveData = GameManager.Instance.CurrentActiveUser.Saves.ToList();
    }
}
