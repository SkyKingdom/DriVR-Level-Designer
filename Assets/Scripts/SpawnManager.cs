using System;
using Actions;
using Interfaces;
using Managers;
using Objects;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class SpawnManager : StaticInstance<SpawnManager>
{
    [SerializeField] private EditMode editMode = EditMode.Object;
    public EditMode EditMode => editMode;
    
    [SerializeField] private ObjectBase prefabToSpawn;
    public ObjectBase PrefabToSpawn => prefabToSpawn;

    // Toggle handling
    private ObjectBase _clickedObject;
    private Toggle _activeToggle;
    
    private InputManager _inputManager;
    private ObjectInspector _objectInspector;
    
    public event Action<IEditorInteractable> ObjectSpawned;
    
    #region Spawning Methods

    public void SpawnObject(Vector3 pos)
    {
        if (prefabToSpawn == null) return;
        
        var spawnedObject = Instantiate(prefabToSpawn, pos, Quaternion.identity);
        spawnedObject.Initialize(spawnedObject.gameObject.name, prefabToSpawn.name);
        var action = new SpawnAction( _objectInspector, spawnedObject);
        ActionRecorder.Instance.Record(action);
        ObjectSpawned?.Invoke(spawnedObject);
    }

    private void SpawnPathPoint(Vector3 pos)
    {
        PathManager.Instance.HandlePathPointSpawn(pos);
    }

    public void AddRoadPoint(Vector3 pos)
    {
        var action = new RoadPointAction(pos);
        ActionRecorder.Instance.Record(action);
    }

    #endregion

    #region Mode Methods
    
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
    
    #endregion
}

