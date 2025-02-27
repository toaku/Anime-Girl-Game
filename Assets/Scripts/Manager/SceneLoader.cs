using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private static SceneLoader _instance;
    public static SceneLoader instance
    {
        get
        {
            if(_instance == null)
                _instance = new SceneLoader();

            return _instance;
        }
    }

    private SceneLoader() {}

    public void LoadBattleScene()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
