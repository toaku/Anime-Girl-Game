using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static VolumeManager;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    private VolumeManager volumeManger;
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private Volume volume;

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged()
    {
        volumeManger.ChangeVolume(volume, volumeSlider.value);
    }
}
