using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestGameStartButton : MainMenuSceneTest
{
    private GameStartButton gameStartButton;

    protected override void OnSceneLoadingEnd()
    {
        gameStartButton = GetGameStartButton();
    }

    [UnityTest]
    public IEnumerator TestGameStart()
    {
        gameStartButton.GameStart();

        yield return null;

        Assert.AreEqual("BattleScene", SceneManager.GetActiveScene().name);
    }
}
