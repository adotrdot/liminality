using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmptySaveSlotContainer : MonoBehaviour
{
    public string SaveInfoText;
    public int SaveIndex;
    public Action OnSave;

    private TextMeshProUGUI m_saveInfoUI;
    private Button m_saveButton;

    // Start is called before the first frame update
    void Start()
    {
        m_saveInfoUI = GetComponentInChildren<TextMeshProUGUI>();
        m_saveButton = GetComponentInChildren<Button>();

        m_saveInfoUI.text = SaveInfoText;
        m_saveButton.onClick.AddListener(() => OnSave.Invoke());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSaveInfo(int index)
    {
        SaveIndex = index++;

        SaveInfoText = "#" + index.ToString() + " empty";

        if (m_saveInfoUI != null)
            m_saveInfoUI.text = SaveInfoText;
    }
}
