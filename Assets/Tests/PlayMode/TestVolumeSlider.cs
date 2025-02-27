using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.TestTools;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TestVolumeSlider : BattleSceneTest
{
    private VolumeSlider volumeSlider;

    protected override void OnSceneLoadingEnd()
    {
        GetPanelController().gameObject.SetActive(true);
        volumeSlider = GetVolumeSlider();
    }

    [Test]
    public void TestOnValueChanged()
    {
        VolumeManager.Volume volume = (VolumeManager.Volume)PrivateMemberAccessor.GetField(volumeSlider, "volume");
        VolumeManager volumeManager = (VolumeManager)PrivateMemberAccessor.GetField(volumeSlider, "volumeManager");
        AudioMixer master = (AudioMixer)PrivateMemberAccessor.GetField(volumeManager, "master");

        volumeSlider.GetComponent<Slider>().value = 0.1f;
        volumeSlider.OnValueChanged();

        float decibel = 0f;
        master.GetFloat(Enum.GetName(typeof(VolumeManager.Volume), volume), out decibel);

        Assert.AreEqual(-20f, decibel);
    }
}
