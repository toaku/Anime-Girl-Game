using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public abstract class BattleSceneTest : SceneBaseTest
{
    public override void LoadScene()
    {
        SceneLoader.instance.LoadBattleScene();
    }

    protected Player GetPlayer()
    {
        return GameObject.Find("Player").GetComponent<Player>();
    }

    protected PlayerCamera GetPlayerCamera()
    {
        return GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>();
    }

    protected MobileController GetMobileController()
    {
        return GameObject.Find("MobileController").GetComponent<MobileController>();
    }

    protected Joystick GetJoystick()
    {
        return GameObject.Find("Joystick").GetComponent<Joystick>();
    }

    protected PanelController GetPanelController()
    {
        return GameObject.Find("Screen Canvas").transform.Find("Setting Panel").GetComponent<PanelController>();
    }

    protected MainMenuButton GetMainMenuButton()
    {
        return GameObject.Find("MainMenu Button").GetComponent<MainMenuButton>();
    }

    protected VolumeManager GetVolumeManager()
    {
        return GameObject.Find("VolumeManager").GetComponent<VolumeManager>();
    }

    protected VolumeSlider GetVolumeSlider()
    {
        return GameObject.Find("Master Volume").GetComponent<VolumeSlider>();
    }

    protected Goblin GetGoblin()
    {
        return GameObject.Find("Goblin").GetComponent<Goblin>();
    }
}
