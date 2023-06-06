using System;
using Managers;
using Mapbox.Examples;
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

public class DesignerManager : StaticInstance<DesignerManager>
{
    #region Managers

    [field: SerializeField, Header("Dependencies")] public DesignerInterfaceManager DesignerUIManager { get; private set; }
    [field: SerializeField] public MapManager MapManager { get; private set; }

    #endregion

    private ModeBase activeMode;
    [SerializeField] private Mode currentMode = Mode.View;
    public Mode CurrentMode => currentMode;
    
    public event Action<Mode, Mode> OnModeChange;
    
    [Header("FPS Mode")]
    [SerializeField] private GameObject sceneCamera;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject capsule;
    [SerializeField] private Toggle mapToggle;

    [Header("Objects Blanket")] 
    [SerializeField] private GameObject blanket;

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
        _mapMode = new MapMode(MapManager.Map , FindObjectOfType<CameraController>());
        _editMode = new EditMode(FindObjectOfType<SpawnManager>(), blanket);
        _viewMode = new ViewMode();
        _firstPersonMode = new FirstPersonMode(sceneCamera, mainCamera, canvas, capsule);
        SetMode((int)Mode.Edit);
    }
    
    private void ChangeMode(ModeBase newMode)
    {
        activeMode?.OnExit();
        activeMode = newMode;
        activeMode?.OnEnter();
    }

    public void SetMode(int modeIndex)
    {
        var oldMode = currentMode;
        switch ((Mode)modeIndex)
        {
            case Mode.Map:
                ChangeMode(_mapMode);
                currentMode = (Mode)modeIndex;
                break;
            case Mode.Edit:
                ChangeMode(_editMode);
                currentMode = (Mode)modeIndex;
                break;
            case Mode.View:
                ChangeMode(_viewMode);
                currentMode = (Mode)modeIndex;
                break;
            case Mode.FirstPerson:
                ChangeMode(_firstPersonMode);
                currentMode = (Mode)modeIndex;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(modeIndex), modeIndex, null);
        }

        OnModeChange?.Invoke(oldMode, currentMode);
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