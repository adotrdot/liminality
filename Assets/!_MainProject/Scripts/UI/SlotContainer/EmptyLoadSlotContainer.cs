using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmptyLoadSlotContainer : MonoBehaviour
{
    public string LoadInfoText;
    public int LoadIndex;

    private TextMeshProUGUI m_loadInfoUI;

    // Start is called before the first frame update
    void Start()
    {
        m_loadInfoUI = GetComponentInChildren<TextMeshProUGUI>();
        m_loadInfoUI.text = LoadInfoText;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLoadInfo(int index)
    {
        LoadIndex = index++;

        LoadInfoText = "#" + index.ToString() + " empty";

        if (m_loadInfoUI != null)
            m_loadInfoUI.text = LoadInfoText;
    }
}
