using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestMainMenuButton : BattleSceneTest
{
    private MainMenuButton mainMenuButton;

    protected override void OnSceneLoadingEnd()
    {
        mainMenuButton = GetMainMenuButton();
    }

    [UnityTest]
    public IEnumerator TestLoadMainMenu()
    {
        mainMenuButton.LoadMainMenu();

        yield return null;

        Assert.AreEqual("MainMenu", SceneManager.GetActiveScene().name);
    }
}
