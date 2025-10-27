using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddUserMenu : MonoBehaviour
{
    #region Fields and properties

    public UserManagementMenu UserManagementMenu;
    [SerializeField] private TMP_InputField m_usernameInput;
    [SerializeField] private Button m_confirmButton;

    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        // Initially hide UI
        HideMenu();

        // Connect listeners
        m_confirmButton.onClick.AddListener(Confirm);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Public methods

    public void ShowMenu()
    {
        ClearInput();
        this.gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void ClearInput()
    {
        m_usernameInput.text = "";
    }

    #endregion
    
    #region Private methods
    
    private void Confirm()
    {
        // Validate if username is empty
        string username = m_usernameInput.text;
        if (username == string.Empty)
        {
            Debug.LogWarning("Username tidak boleh kosong");
        }
        else
        {
            UserDatabase.Instance.AddUser(username);
            UserManagementMenu.RefreshList();
            HideMenu();
        }
    }

    #endregion
}
