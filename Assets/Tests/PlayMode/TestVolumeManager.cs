using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.TestTools;

public class TestVolumeManager : BattleSceneTest
{
    private VolumeManager volumeManager;

    protected override void OnSceneLoadingEnd()
    {
        volumeManager = GetVolumeManager();
    }

    [Test]
    public void TestChangeVolume()
    {
        AudioMixer master = (AudioMixer)PrivateMemberAccessor.GetField(volumeManager, "master");
        float decibel = 0f;

        volumeManager.ChangeVolume(VolumeManager.Volume.MasterVolume, 0.1f);

        master.GetFloat(Enum.GetName(typeof(VolumeManager.Volume), VolumeManager.Volume.MasterVolume), out decibel);

        Assert.AreEqual(-20f, decibel);
    }
}
