using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer master;

    public enum Volume
    {
        MasterVolume,
        BGMVolume,
        SFXVolume
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeVolume(Volume volume, float linearDecibel)
    {
        float decibel = Mathf.Log10(linearDecibel) * 20;
        master.SetFloat(Enum.GetName(typeof(Volume), volume), decibel);
    }
}
