using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadSlotContainer : MonoBehaviour
{
    public string LoadInfoText;
    public int LoadIndex;
    public Action OnLoad;

    private TextMeshProUGUI m_loadInfoUI;
    private Button m_loadButton;

    // Start is called before the first frame update
    void Start()
    {
        m_loadInfoUI = GetComponentInChildren<TextMeshProUGUI>();
        m_loadButton = GetComponentInChildren<Button>();

        m_loadInfoUI.text = LoadInfoText;
        m_loadButton.onClick.AddListener(() => OnLoad.Invoke());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSaveInfo(int index, SaveData saveData)
    {
        LoadIndex = index++;

        string saveIndex = "#" + index.ToString() + " ";
        string saveTime = saveData.SaveTime.ToString("yyyy-MM-dd HH:mm");
        LoadInfoText = saveIndex + saveTime;

        if (m_loadInfoUI != null)
            m_loadInfoUI.text = LoadInfoText;
    }
}
