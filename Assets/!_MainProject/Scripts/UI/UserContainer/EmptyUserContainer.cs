using System;
using UnityEngine;
using UnityEngine.UI;

public class EmptyUserContainer : MonoBehaviour
{
    public Action OnCreateButton;
    private Button m_createButton;

    // Start is called before the first frame update
    void Start()
    {
        m_createButton = GetComponentInChildren<Button>();
        m_createButton.onClick.AddListener(() => OnCreateButton.Invoke());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
