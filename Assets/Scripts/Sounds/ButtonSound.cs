using System;
using System.Collections;
using System.Collections.Generic;
using JSAM;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public GeneratorMusicSounds buttonSounds;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        AudioManager.PlaySound(buttonSounds);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
