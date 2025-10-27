using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    #region Fields and properties

    public SaveMenu SaveMenu;

    #endregion

    #region Monobehaviour methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.PauseAction.WasPressedThisFrame())
        {
            PauseGame();
        }
    }

    #endregion

    #region Public methods

    public void SaveGame(int index)
    {
        SaveData newSaveData = new SaveData();
        newSaveData.EndingScoreA = GameManager.Instance.EndingScoreA;
        newSaveData.EndingScoreB = GameManager.Instance.EndingScoreB;
        newSaveData.NarrativeDataIndex = GameManager.Instance.CurrentNarrativeDataIndex;
        newSaveData.SaveTime = DateTime.Now;

        GameManager.Instance.CurrentActiveUser.Saves[index] = newSaveData;
        UserDatabase.Instance.Save();
    }

    #endregion

    /// <summary>
    /// Method to pause game and show save menu.
    /// </summary>
    private void PauseGame()
    {
        SaveMenu.ShowMenu();
    }
}
