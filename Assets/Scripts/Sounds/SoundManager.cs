using System.Collections;
using System.Collections.Generic;
using JSAM;
using UnityEngine;

/// <summary>
/// Plays background music
/// </summary>
public class SoundManager : MonoBehaviour
{
    public float volume = 0.5f;
    void Start()
    {
        AudioManager.PlayMusic(GeneratorMusicMusic.BackgroundMusic);
        AudioManager.SetMusicVolume(volume);
    }
}
