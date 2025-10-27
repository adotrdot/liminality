using UnityEngine;
using UnityEngine.UI;

public class UserManagementMenu : MonoBehaviour
{
    #region Fields and properties

    public MainMenu MainMenu;
    public AddUserMenu AddUserMenu;
    public GameObject UserAccountBox1;
    public GameObject UserAccountBox2;
    public GameObject EmptyUserContainerPrefab;
    public GameObject FilledUserContainerPrefab;

    [SerializeField] private Button m_closeButton;

    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        // Initially disable this
        HideMenu();

        // Add button listeners
        m_closeButton.onClick.AddListener(HideMenu);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Public methods

    public void ShowMenu()
    {
        RefreshList();
        this.gameObject.SetActive(true);
    }

    public void HideMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void SpawnAddUserMenu()
    {
        AddUserMenu.ShowMenu();
    }

    public void RefreshList()
    {
        ClearList();

        int userCount = UserDatabase.Instance.Users.Count;
        if (userCount == 0)
        {
            // Both boxes have empty container

            // Instantiate
            EmptyUserContainer emptyUser1 = Instantiate(EmptyUserContainerPrefab, UserAccountBox1.transform)
                                    .GetComponent<EmptyUserContainer>();
            EmptyUserContainer emptyUser2 = Instantiate(EmptyUserContainerPrefab, UserAccountBox2.transform)
                                    .GetComponent<EmptyUserContainer>();

            // Connect actions
            emptyUser1.OnCreateButton = SpawnAddUserMenu;
            emptyUser2.OnCreateButton = SpawnAddUserMenu;
        }
        else if (userCount == 1)
        {
            // Box 1 have user, box 2 have empty container

            // Instantiate
            UserContainer filledUser1 = Instantiate(FilledUserContainerPrefab, UserAccountBox1.transform)
                                                    .GetComponent<UserContainer>();
            EmptyUserContainer emptyUser1 = Instantiate(EmptyUserContainerPrefab, UserAccountBox2.transform)
                                                    .GetComponent<EmptyUserContainer>();

            // Fill users
            filledUser1.SetUsername(UserDatabase.Instance.Users[0].Username);

            // Connect actions
            filledUser1.OnSelect = () => OnSelectUser(0);
            filledUser1.OnDelete = () => OnDeleteUser(0);
            emptyUser1.OnCreateButton = SpawnAddUserMenu;
        }
        else
        {
            // Both boxes have filled user container

            // Instantiate
            UserContainer filledUser1 = Instantiate(FilledUserContainerPrefab, UserAccountBox1.transform)
                                                    .GetComponent<UserContainer>();
            UserContainer filledUser2 = Instantiate(FilledUserContainerPrefab, UserAccountBox2.transform)
                                                    .GetComponent<UserContainer>();

            // Fill users
            filledUser1.SetUsername(UserDatabase.Instance.Users[0].Username);
            filledUser2.SetUsername(UserDatabase.Instance.Users[1].Username);

            // Connect actions
            filledUser1.OnSelect = () => OnSelectUser(0);
            filledUser1.OnDelete = () => OnDeleteUser(0);
            filledUser2.OnSelect = () => OnSelectUser(1);
            filledUser2.OnDelete = () => OnDeleteUser(1);
        }
    }

    #endregion

    #region Private methods

    private void OnSelectUser(int index)
    {
        GameManager.Instance.SetActiveUser(UserDatabase.Instance.Users[index]);
        HideMenu();
        MainMenu.RefreshMainMenu();
    }

    private void OnDeleteUser(int index)
    {
        UserDatabase.Instance.DeleteUser(index);
        RefreshList();
    }

    private void ClearList()
    {
        if (UserAccountBox1.transform.childCount > 0)
            Destroy(UserAccountBox1.transform.GetChild(0).gameObject);
        
        if (UserAccountBox2.transform.childCount > 0)
            Destroy(UserAccountBox2.transform.GetChild(0).gameObject);
    }

    #endregion
}
