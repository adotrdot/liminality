using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{
    #region Fields and properties

    public List<GameObject> LoadSlotBoxes;
    public GameObject EmptyLoadContainerPrefab;
    public GameObject FilledLoadContainerPrefab;

    [SerializeField] private Button m_closeButton;

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
                EmptyLoadSlotContainer loadSlotContainer = Instantiate(EmptyLoadContainerPrefab, LoadSlotBoxes[j].transform)
                                            .GetComponent<EmptyLoadSlotContainer>();
                loadSlotContainer.SetLoadInfo(j);
            }
            else
            {
                // Filled save
                LoadSlotContainer loadSlotContainer = Instantiate(FilledLoadContainerPrefab, LoadSlotBoxes[j].transform)
                                                        .GetComponent<LoadSlotContainer>();
                loadSlotContainer.SetSaveInfo(j, m_mySaveData[j]);
                loadSlotContainer.OnLoad = () =>
                {
                    HideMenu();
                    GameManager.Instance.LoadGame(m_mySaveData[j]);
                };
            }
        }
    }

    public void ClearList()
    {
        foreach (GameObject loadSlot in LoadSlotBoxes)
        {
            if (loadSlot.transform.childCount > 0)
                Destroy(loadSlot.transform.GetChild(0).gameObject);
        }
    }
    
    private void GetMySaveData()
    {
        m_mySaveData = GameManager.Instance.CurrentActiveUser.Saves.ToList();
    }
}
