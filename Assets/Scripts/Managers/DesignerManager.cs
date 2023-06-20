using System;
using Managers;
using Mapbox.Examples;
using Mapbox.Utils;
using Saving;
using TransformHandles;
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
public enum EditMode
{
    Object = 0,
    Path = 1,
    Road = 2,
    None = 3,
}

[DefaultExecutionOrder(-100)]
public class DesignerManager : StaticInstance<DesignerManager>
{
    #region Managers

    [field: SerializeField, Header("Dependencies")] public DesignerInterfaceManager DesignerUIManager { get; private set; }
    [field: SerializeField] public MapManager MapManager { get; private set; }
    
    [field: SerializeField] public InputManager InputManager { get; private set; }
    
    [field: SerializeField] public SpawnManager SpawnManager { get; private set; }
    
    [field: SerializeField] public SelectionManager SelectionManager { get; private set; }
    
    [field: SerializeField] public TransformHandleManager TransformHandleManager { get; private set; }
    [field: SerializeField] public RoadTool RoadTool { get; private set; }
    [field: SerializeField] public PathManager PathManager { get; private set; }

    #endregion
    
    // Stores the current mode
    public Mode CurrentMode { get; private set; }
    public EditMode CurrentEditMode { get; private set; }
    
    // Event for when the mode changes
    public event Action<Mode, Mode> OnModeChange;
    public event Action<EditMode, EditMode> OnEditTypeChange;
    
    private void Start()
    {
        SetMode((int)Mode.Edit); // Default to Edit mode
        SetEditMode((int)EditMode.Object); // Default to Object edit mode
    }
    
    // Sets the current mode
    public void SetMode(int modeIndex)
    {
        var oldMode = CurrentMode;
        CurrentMode = (Mode)modeIndex;
        // Event passes the old mode and the new mode
        OnModeChange?.Invoke(oldMode, CurrentMode);
    }
    
    public void SetEditMode(int editModeIndex)
    {
        var oldMode = CurrentEditMode;
        CurrentEditMode = (EditMode)editModeIndex;
        // Event passes the old mode and the new mode
        OnEditTypeChange?.Invoke(oldMode, CurrentEditMode);
    }

    // Exits to the main menu
    public async void ExitToMenu()
    {
        await LevelDataManager.Instance.Cleanup();
        SceneManager.LoadScene(0);
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

    public void ToggleRoadEdit(bool value)
    {
        if (value)
        {
            SetEditMode((int) EditMode.Road);
            return;
        }
        
        SetEditMode((int) EditMode.Object);
    }
    
    public void TogglePathMode(bool value)
    {
        if (value)
        {
            SetEditMode((int) EditMode.Path);
            return;
        }
        
        SetEditMode((int) EditMode.Object);
    }
    
    public bool InEditType(EditMode editMode)
    {
        return CurrentEditMode == editMode;
    }
}