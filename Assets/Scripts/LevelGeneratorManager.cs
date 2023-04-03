using System;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Saving;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

[Serializable]
public enum Mode
{
    View = 0,
    Edit = 1,
    Map = 2,
    FirstPerson = 3
}

public class LevelGeneratorManager : StaticInstance<LevelGeneratorManager>
{
    public ModeBase CurrentMode { get; private set; }
    [SerializeField] private Mode mode = Mode.View;
    public Mode Mode => mode;
    [SerializeField] private bool mapEnabled;
    public bool MapEnabled => mapEnabled;
    
    public event Action<Mode> OnModeChange;
    
    [SerializeField] UiNavigation uiNavigation;
    
    [Header("FPS Mode")]
    [SerializeField] private GameObject sceneCamera;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject capsule;
    [SerializeField] private GameObject plane;
    [SerializeField] private Toggle mapToggle;

    public Transform SceneCameraTransform => sceneCamera.transform;
    // Modes
    private MapMode _mapMode;
    public MapMode MapMode => _mapMode;
    private EditMode _editMode;
    public EditMode EditMode => _editMode;
    private ViewMode _viewMode;
    public ViewMode ViewMode => _viewMode;

    private FirstPersonMode _firstPersonMode;
    public FirstPersonMode FirstPersonMode => _firstPersonMode;

    private void Start()
    {
        _mapMode = new MapMode(FindObjectOfType<AbstractMap>() , FindObjectOfType<CameraController>(), plane);
        _editMode = new EditMode(FindObjectOfType<ObjectManager>());
        _viewMode = new ViewMode();
        _firstPersonMode = new FirstPersonMode(sceneCamera, mainCamera, canvas, capsule);
        SetMode(1);
    }

    public void OnMapEnabledValueChange(bool value)
    {
        mapEnabled = value;
        if (!value && CurrentMode == _mapMode)
        {
            ChangeMode(_viewMode);
            uiNavigation.SwitchToMode((int)Mode.Edit);
        }
        uiNavigation.modeButtons[(int)Mode.Map].interactable = value;
        MapMode.ToggleMap(value);
    }

    private void ChangeMode(ModeBase newMode)
    {
        CurrentMode?.OnExit();
        CurrentMode = newMode;
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
                    mode = (Mode)modeIndex;
                }
                else
                {
                    Debug.Log("Map is disabled");
                }
                break;
            case Mode.Edit:
                ChangeMode(_editMode);
                mode = (Mode)modeIndex;
                break;
            case Mode.View:
                ChangeMode(_viewMode);
                mode = (Mode)modeIndex;
                break;
            case Mode.FirstPerson:
                ChangeMode(_firstPersonMode);
                mode = (Mode)modeIndex;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(modeIndex), modeIndex, null);
        }
        OnModeChange?.Invoke(Mode);
    }

    public async void ExitToMenu()
    {
        await LevelDataManager.Instance.Cleanup();
        SceneManager.LoadScene(1);
    }

    public void LoadMap(float zoom, double posX, double posY)
    {
        var center = new Vector2d(posX, posY);
        MapMode.Map.UpdateMap(center, zoom);
        mapToggle.isOn = true;
    }
    
}