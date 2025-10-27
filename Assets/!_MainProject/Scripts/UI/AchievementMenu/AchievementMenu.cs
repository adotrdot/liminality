using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementMenu : MonoBehaviour
{
    #region Fields and Properties

    public GameObject AchievementBox1;
    public GameObject AchievementBox2;
    public GameObject AchievementBox3;
    public GameObject AchievementInfoPrefab;

    [SerializeField] private Button m_closeButton;

    private List<Achievement> m_myAchievements => GameManager.Instance.CurrentActiveUser.Achievements;

    #endregion

    #region Monobehaviour methods

    // Start is called before the first frame update
    void Start()
    {
        // Initially hide this
        HideMenu();

        // Connect listeners
        m_closeButton = GetComponentInChildren<Button>();
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

    public void RefreshList()
    {
        ClearList();

        // Instantiate
        AchievementInfo achievementInfo1 = Instantiate(AchievementInfoPrefab, AchievementBox1.transform)
                                            .GetComponent<AchievementInfo>();
        AchievementInfo achievementInfo2 = Instantiate(AchievementInfoPrefab, AchievementBox2.transform)
                                            .GetComponent<AchievementInfo>();
        AchievementInfo achievementInfo3 = Instantiate(AchievementInfoPrefab, AchievementBox3.transform)
                                            .GetComponent<AchievementInfo>();

        // Initialize achievement infos
        achievementInfo1.SetAchievementInfo(m_myAchievements[0]);
        achievementInfo2.SetAchievementInfo(m_myAchievements[1]);
        achievementInfo3.SetAchievementInfo(m_myAchievements[2]);
    }
    
    public void ClearList()
    {
        if (AchievementBox1.transform.childCount > 0)
            Destroy(AchievementBox1.transform.GetChild(0).gameObject);
        if (AchievementBox2.transform.childCount > 0)
            Destroy(AchievementBox2.transform.GetChild(0).gameObject);
        if (AchievementBox3.transform.childCount > 0)
            Destroy(AchievementBox3.transform.GetChild(0).gameObject);
    }

    #endregion
    
    #region Private methods



    #endregion
}
