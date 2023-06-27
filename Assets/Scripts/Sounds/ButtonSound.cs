using JSAM;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Plays a sound when a button is pressed
/// </summary>
public class ButtonSound : MonoBehaviour
{
    public GeneratorMusicSounds buttonSounds;
    private Button _button;
    private Toggle _toggle;

    private void Awake()
    {
        _button = GetComponent<Button>();
        
        if (!_button)
            _toggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        if (_button)
            _button.onClick.AddListener(PlaySound);
        
        if (_toggle)
            _toggle.onValueChanged.AddListener(PlaySound);
    }

    private void PlaySound(bool arg0)
    {
        if (arg0)
            AudioManager.PlaySound(buttonSounds);
        else
        {
            AudioManager.PlaySound(GeneratorMusicSounds.UIClose);
        }
    }

    private void PlaySound()
    {
        AudioManager.PlaySound(buttonSounds);
    }

    private void OnDestroy()
    {
        if (_button) 
            _button.onClick.RemoveAllListeners();
        
        if (_toggle)
            _toggle.onValueChanged.RemoveAllListeners();
    }
}
