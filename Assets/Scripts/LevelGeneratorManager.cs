using System;
using Mapbox.Unity.Map;
using UnityEngine;
using Utilities;

public class LevelGeneratorManager : StaticInstance<LevelGeneratorManager>
{
    [property: SerializeField] public ModeBase Mode { get; private set; }
    [SerializeField] private bool mapEnabled;
    
    // Modes
    [SerializeField] private MapMode _mapMode;
    public MapMode MapMode => _mapMode;
    [SerializeField] private EditMode _editMode;
    [SerializeField] private ViewMode _viewMode;

    protected override void Awake()
    {
        base.Awake();
        _mapMode = new MapMode(FindObjectOfType<AbstractMap>() , FindObjectOfType<CameraController>());
        _editMode = new EditMode();
        _viewMode = new ViewMode();
    }

    private void Start()
    {
        ChangeMode(_viewMode);
    }

    public void OnMapEnabledValueChange(bool value)
    {
        mapEnabled = value;
    }
    
    public void ChangeMode(ModeBase mode)
    {
        Mode?.OnExit();
        Mode = mode;
        Mode?.OnEnter();
    }
}