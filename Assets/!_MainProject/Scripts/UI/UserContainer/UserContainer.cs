using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserContainer : MonoBehaviour
{
    public Action OnSelect;
    public Action OnDelete;
    public string Username;

    private TextMeshProUGUI m_username;
    private Button m_selectButton;
    private Button m_deleteButton;

    // Start is called before the first frame update
    void Start()
    {
        m_username = GetComponentInChildren<TextMeshProUGUI>();
        List<Button> buttons = GetComponentsInChildren<Button>().ToList();
        m_selectButton = buttons[0];
        m_deleteButton = buttons[1];

        m_username.text = Username;
        m_selectButton.onClick.AddListener(() => OnSelect.Invoke());
        m_deleteButton.onClick.AddListener(() => OnDelete.Invoke());
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void SetUsername(string username)
    {
        Username = username;

        if (m_username != null)
            m_username.text = username;
    }
}
