using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region Fields and properties

    [Header("References to Other Components")]
    public UserManagementMenu UserManagementMenu;

    [Header("Main Control Buttons")]
    [SerializeField] private Button m_startButton;
    [SerializeField] private Button m_loadButton;
    [SerializeField] private Button m_manageEchoesButton;
    [SerializeField] private Button m_exitButton;

    [Header("Miscellaneous UI")]
    [SerializeField] private Button m_achievementButton;
    [SerializeField] private TextMeshProUGUI m_activeUsernameText;

    #endregion

    #region Monobehaviour methods

    private void Start()
    {
        RefreshMainMenu();

        // Connect listeners
        m_startButton.onClick.AddListener(OnStartClicked);
        m_loadButton.onClick.AddListener(OnLoadClicked);
        m_manageEchoesButton.onClick.AddListener(OnManageEchoesClicked);
        m_exitButton.onClick.AddListener(OnExitClicked);
    }

    #endregion

    #region Public methods
    
    public void RefreshMainMenu()
    {
        // Disable Start, Load, Achievement Button, and activeUserText if no active user
        bool hasUser = GameManager.Instance.HasActiveUser;
        m_startButton.interactable = hasUser;
        m_loadButton.interactable = hasUser;
        m_achievementButton.interactable = hasUser;
        string activeUsername = hasUser ? GameManager.Instance.CurrentActiveUser.Username : "";
        if (activeUsername != string.Empty)
        {
            m_activeUsernameText.text = "Active Echo: " + activeUsername;
            m_activeUsernameText.gameObject.SetActive(true);
        }
        else
        {
            m_activeUsernameText.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Private methods

    private void OnStartClicked()
    {
        // Start new game logic
        Debug.Log("Start clicked");
        // GameManager.Instance.StartNewGame();
    }

    private void OnLoadClicked()
    {
        // Load existing save logic
        Debug.Log("Load clicked");
        // Show load UI
    }

    private void OnManageEchoesClicked()
    {
        // Open user management menu popup
        UserManagementMenu.ShowMenu();
    }

    private void OnExitClicked()
    {
        Application.Quit();
    }

    #endregion
}
