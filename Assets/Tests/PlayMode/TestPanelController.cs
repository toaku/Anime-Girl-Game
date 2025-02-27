using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestPanelController : BattleSceneTest
{
    private PanelController panelController;

    protected override void OnSceneLoadingEnd()
    {
        panelController = GetPanelController();
    }

    [Test]
    public void TestOpenPanel()
    {
        ResetPanel();

        panelController.OpenPanel();
        Assert.AreEqual(true, panelController.gameObject.activeSelf);
    }

    [Test]
    public void TestClosePanel()
    {
        ResetPanel();

        panelController.gameObject.SetActive(true);
        panelController.ClosePanel();
        Assert.AreEqual(false, panelController.gameObject.activeSelf);
    }

    private void ResetPanel()
    {
        panelController.gameObject.SetActive(false);
    }
}
