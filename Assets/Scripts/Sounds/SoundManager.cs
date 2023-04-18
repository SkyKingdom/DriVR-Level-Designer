using System.Collections;
using System.Collections.Generic;
using JSAM;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float volume = 0.5f;
    void Start()
    {
        AudioManager.PlayMusic(GeneratorMusicMusic.BackgroundMusic);
        AudioManager.SetMusicVolume(volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
