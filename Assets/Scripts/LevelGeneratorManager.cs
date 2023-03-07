using System;
using Mapbox.Unity.Map;
using UnityEngine;
using Utilities;

[Serializable]
public enum Mode
{
    View = 0,
    Edit = 1,
    Map = 2
}

public class LevelGeneratorManager : StaticInstance<LevelGeneratorManager>
{
    public ModeBase CurrentMode { get; private set; }
    [SerializeField] private Mode mode = Mode.View;
    public Mode Mode => mode;
    [SerializeField] private bool mapEnabled;
    public bool MapEnabled => mapEnabled;
    
    [SerializeField] UiNavigation uiNavigation;
    
    // Modes
    private MapMode _mapMode;
    public MapMode MapMode => _mapMode;
    private EditMode _editMode;
    public EditMode EditMode => _editMode;
    private ViewMode _viewMode;
    public ViewMode ViewMode => _viewMode;

    private void Start()
    {
        _mapMode = new MapMode(FindObjectOfType<AbstractMap>() , FindObjectOfType<CameraController>());
        _editMode = new EditMode(FindObjectOfType<ObjectManager>());
        _viewMode = new ViewMode();
        ChangeMode(_editMode);
    }

    public void OnMapEnabledValueChange(bool value)
    {
        mapEnabled = value;
        if (!value && CurrentMode == _mapMode)
        {
            ChangeMode(_viewMode);
            uiNavigation.SwitchToMode((int)Mode.View);
        }
        uiNavigation.modeButtons[(int)Mode.Map].interactable = value;
        MapMode.ToggleMap(value);
    }

    private void ChangeMode(ModeBase mode)
    {
        CurrentMode?.OnExit();
        CurrentMode = mode;
        CurrentMode?.OnEnter();
    }

    public void SetMode(int modeIndex)
    {
        switch ((Mode)modeIndex)
        {
            case Mode.Map:
                if (MapEnabled)
                {
                    ChangeMode(_mapMode);
                }
                else
                {
                    Debug.Log("Map is disabled");
                }
                break;
            case Mode.Edit:
                ChangeMode(_editMode);
                break;
            case Mode.View:
                ChangeMode(_viewMode);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(modeIndex), modeIndex, null);
        }
    }
}