using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotContainer : MonoBehaviour
{
    public string SaveInfoText;
    public int SaveIndex;
    public Action OnSave;
    public Action OnLoad;

    private TextMeshProUGUI m_saveInfoUI;
    private Button m_saveButton;
    private Button m_loadButton;

    // Start is called before the first frame update
    void Start()
    {
        List<Button> buttons = GetComponentsInChildren<Button>().ToList();
        m_saveInfoUI = GetComponentInChildren<TextMeshProUGUI>();
        m_saveButton = buttons[0];
        m_loadButton = buttons[1];

        m_saveInfoUI.text = SaveInfoText;
        m_saveButton.onClick.AddListener(() => OnSave.Invoke());
        m_loadButton.onClick.AddListener(() => OnLoad.Invoke());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSaveInfo(int index, SaveData saveData)
    {
        SaveIndex = index++;

        string saveIndex = "#" + index.ToString() + " ";
        string saveTime = saveData.SaveTime.ToString("yyyy-MM-dd HH:mm");
        SaveInfoText = saveIndex + saveTime;

        if (m_saveInfoUI != null)
            m_saveInfoUI.text = SaveInfoText;
    }
}
