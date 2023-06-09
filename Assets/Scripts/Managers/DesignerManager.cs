using System;
using Managers;
using Mapbox.Examples;
using Mapbox.Utils;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

[Serializable]
public enum Mode
{
    View = 0,
    Edit = 1,
    Map = 2,
    FirstPerson = 3
}

[Serializable]
public enum EditType
{
    Object = 0,
    Path = 1,
    Road = 2
}

public class DesignerManager : StaticInstance<DesignerManager>
{
    #region Managers

    [field: SerializeField, Header("Dependencies")] public DesignerInterfaceManager DesignerUIManager { get; private set; }
    [field: SerializeField] public MapManager MapManager { get; private set; }
    
    [field: SerializeField] public InputManager InputManager { get; private set; }

    #endregion
    
    // Stores the current mode
    public Mode CurrentMode { get; private set; }
    
    // Event for when the mode changes
    public event Action<Mode, Mode> OnModeChange;
    
    private void Start()
    {
        SetMode((int)Mode.Edit); // Default to Edit mode
    }
    
    // Sets the current mode
    public void SetMode(int modeIndex)
    {
        var oldMode = CurrentMode;
        CurrentMode = (Mode)modeIndex;
        // Event passes the old mode and the new mode
        OnModeChange?.Invoke(oldMode, CurrentMode);
    }

    // Exits to the main menu
    public async void ExitToMenu()
    {
        await LevelDataManager.Instance.Cleanup();
        SceneManager.LoadScene(1);
    }

    // Loads the map with the given zoom and position
    public void LoadMap(float zoom, double posX, double posY)
    {
        MapManager.ToggleMap(true);
        var center = new Vector2d(posX, posY);
        MapManager.Map.Initialize(center, (int)zoom);
    }
    
    // Enters first person mode
    public void ToggleFPS()
    {
        SetMode((int)Mode.FirstPerson);
    }
}