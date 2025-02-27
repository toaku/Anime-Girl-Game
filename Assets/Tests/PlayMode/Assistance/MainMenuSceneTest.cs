using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public abstract class MainMenuSceneTest : SceneBaseTest
{
    public override void LoadScene()
    {
        SceneLoader.instance.LoadMainMenu();
    }

    protected GameStartButton GetGameStartButton()
    {
        return GameObject.Find("Game Start Button").GetComponent<GameStartButton>();
    }
}
