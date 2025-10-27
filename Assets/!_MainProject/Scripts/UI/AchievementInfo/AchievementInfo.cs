using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementInfo : MonoBehaviour
{
    public string AchievementInfoText;
    private TextMeshProUGUI m_achievementTextUI;

    // Start is called before the first frame update
    void Start()
    {
        m_achievementTextUI = GetComponent<TextMeshProUGUI>();
        m_achievementTextUI.text = AchievementInfoText;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void SetAchievementInfo(Achievement achievement)
    {
        string achievementName = achievement.Name;
        string lockStatus = achievement.IsUnlocked ? " (Achieved)" : " (Not Yet Achieved)";
        string achievementInfo = achievementName + lockStatus;

        AchievementInfoText = achievementInfo;

        if (m_achievementTextUI != null)
            m_achievementTextUI.text = achievementInfo;
    }
}
