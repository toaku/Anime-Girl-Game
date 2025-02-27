using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestSceneLoader
{
    [UnityTest]
    public IEnumerator TestLoadBattleScene()
    {
        SceneLoader.instance.LoadBattleScene();
        yield return null;
        Assert.AreEqual("BattleScene", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator TestLoadMainMenu()
    {
        SceneLoader.instance.LoadMainMenu();
        yield return null;
        Assert.AreEqual("MainMenu", SceneManager.GetActiveScene().name);
    }
}
