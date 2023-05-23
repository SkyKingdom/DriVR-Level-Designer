using System;
using Actions;
using Interfaces;
using Objects;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class SpawnManager : StaticInstance<SpawnManager>
{
    [SerializeField] private EditType editType = EditType.Object;
    public EditType EditType => editType;
    
    [SerializeField] private ObjectBase prefabToSpawn;
    public ObjectBase PrefabToSpawn => prefabToSpawn;

    // Toggle handling
    private ObjectBase _clickedObject;
    private Toggle _activeToggle;
    
    private InputHandler _inputHandler;
    private SettingsManager _settingsManager;
    
    public event Action<IEditorInteractable> ObjectSpawned;
    public event Action<EditType, EditType> EditTypeChanged;
        

    #region Unity Methods

    private void OnEnable()
    {
        if (_settingsManager == null)
        {
            _settingsManager = FindObjectOfType<SettingsManager>();
        }
        if (_inputHandler == null)
        {
            _inputHandler = FindObjectOfType<InputHandler>();
            if (_inputHandler)
                _inputHandler.OnSpawn += OnSpawn;
            return;
        } 
        _inputHandler.OnSpawn += OnSpawn;
    }

    private void OnDisable()
    {
        _inputHandler.OnSpawn -= OnSpawn;
    }

    #endregion
    
    private void OnSpawn(Vector3 position)
    {
        switch (editType)
        {
            case EditType.Object:
                SpawnObject(position);
                break;
            case EditType.Path:
                SpawnPathPoint(position);
                break;
            case EditType.Road:
                SpawnRoad(position);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #region Spawning Methods

    private void SpawnObject(Vector3 pos)
    {
        if (prefabToSpawn == null) return;
        
        var spawnedObject = Instantiate(prefabToSpawn, pos, Quaternion.identity);
        spawnedObject.Initialize(spawnedObject.gameObject.name, prefabToSpawn.name);
        var action = new SpawnAction( _settingsManager, spawnedObject);
        ActionRecorder.Instance.Record(action);
        ObjectSpawned?.Invoke(spawnedObject);
    }

    private void SpawnPathPoint(Vector3 pos) => PathManager.Instance.HandlePathPointSpawn(pos);

    private void SpawnRoad(Vector3 pos)
    {
        var action = new RoadPointAction(pos);
        ActionRecorder.Instance.Record(action);
    }

    #endregion

    #region Mode Methods

    public void ToggleRoadTool(bool value)
    {
        HandleEditTypeChange(value ? EditType.Road : EditType.Object);
    }
    
    public void SelectObject(ObjectBase prefab)
    {
        _clickedObject = prefab;
    }
    
    public void SelectToggle(Toggle toggle)
    {
        _activeToggle = toggle;
    }

    public void HandleObjectToggle(bool value)
    {
        prefabToSpawn = value ? _clickedObject : null;
        if (!value)
            _activeToggle = null;
    }
    
    public void DisableActiveToggle()
    {
        if (!_activeToggle) return;
        _activeToggle.isOn = false;
    }
    
    public void TogglePathTool()
    {
        HandleEditTypeChange(editType == EditType.Path ? EditType.Object : EditType.Path);
    }

    public void HandleEditTypeChange(EditType type)
    {
        EditTypeChanged?.Invoke(editType, type);
        editType = type;
        switch (type)
        {
            case EditType.Object:
                break;
            case EditType.Path:
                prefabToSpawn = null;
                break;
            case EditType.Road:
                prefabToSpawn = null;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    #endregion
}

[Serializable]
public enum EditType
{
    Object,
    Path,
    Road
}