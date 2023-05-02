using System;
using Actions;
using Interfaces;
using Objects;
using SplineMesh;
using UnityEngine;
using Utilities;

public class ObjectManager : StaticInstance<ObjectManager>
{
    [SerializeField] private ObjectBase prefabToSpawn;
    [SerializeField] private RoadTool roadTool;
    public ObjectBase PrefabToSpawn => prefabToSpawn;
    private InputHandler _inputManager;
    private SettingsManager _settingsManager;
    private bool _roadToolEnabled;
    public bool RoadToolEnabled => _roadToolEnabled;

    public event Action<IEditorInteractable> ObjectSpawned; 

    public void SelectObject(ObjectBase prefab)
    {
        if (LevelGeneratorManager.Instance.Mode != Mode.Edit) return;
        prefabToSpawn = PrefabToSpawn == prefab ? null : prefab;
    }
    
    public ObjectBase Spawn(Vector3 position)
    {
        var spawnedObject = Instantiate(prefabToSpawn, position, Quaternion.identity);
        spawnedObject.Initialize(spawnedObject.gameObject.name, prefabToSpawn.name);
        if (_settingsManager == null)
        {
            _settingsManager = FindObjectOfType<SettingsManager>();
        }
        _settingsManager.SelectObject(spawnedObject);
        ObjectSpawned?.Invoke(spawnedObject);
        return spawnedObject;
    }
    
    public void OnSpawnObject(Vector3 position)
    {
        if (_roadToolEnabled)
        {
            position.y = 0;
            var roadAction = new RoadPointAction(roadTool, position);
            ActionRecorder.Instance.Record(roadAction);
            Debug.Log("Placing road point");
            return;
        }
        if (prefabToSpawn == null) return;
        if (_settingsManager == null)
        {
            _settingsManager = FindObjectOfType<SettingsManager>();
        }
        var action = new SpawnAction(this, _settingsManager ,position);
        ActionRecorder.Instance.Record(action);
    }
    
    public void ToggleRoadTool(bool value)
    {
        _roadToolEnabled = value;
        if (value)
        {
            prefabToSpawn = null;
        }
        Debug.Log("Road tool enabled: " + value);
    }

    private void OnEnable()
    {
        if (_inputManager == null)
        {
            _inputManager = FindObjectOfType<InputHandler>();
            if (_inputManager)
                _inputManager.OnSpawnClick += OnSpawnObject;
            return;
        } 
        _inputManager.OnSpawnClick += OnSpawnObject;
    }

    private void OnDisable()
    {
        _inputManager.OnSpawnClick -= OnSpawnObject;
    }
}