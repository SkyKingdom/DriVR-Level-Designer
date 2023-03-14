using Actions;
using Objects;
using UnityEngine;
using Utilities;

public class ObjectManager : StaticInstance<ObjectManager>
{
    [SerializeField] private ObjectBase prefabToSpawn;
    public ObjectBase PrefabToSpawn => prefabToSpawn;
    private InputManager _inputManager;
    private SettingsManager _settingsManager;
    
    public void SelectObject(ObjectBase prefab)
    {
        prefabToSpawn = prefab;
    }
    
    public void ClearObject()
    {
        prefabToSpawn = null;
    }

    public ObjectBase Spawn(Vector3 position)
    {
        var spawnedObject = Instantiate(prefabToSpawn, position, Quaternion.identity);
        spawnedObject.objectName = spawnedObject.gameObject.name;
        if (_settingsManager == null)
        {
            _settingsManager = FindObjectOfType<SettingsManager>();
        }
        _settingsManager.SelectObject(spawnedObject);
        ClearObject();
        return spawnedObject;
    }
    
    public void OnSpawnObject(Vector3 position)
    {
        if (prefabToSpawn == null) return;
        if (_settingsManager == null)
        {
            _settingsManager = FindObjectOfType<SettingsManager>();
        }
        var action = new SpawnAction(this, _settingsManager ,position);
        ActionRecorder.Instance.Record(action);
    }

    private void OnEnable()
    {
        if (_inputManager == null)
        {
            _inputManager = FindObjectOfType<InputManager>();
            if (_inputManager)
                _inputManager.OnMapClick += OnSpawnObject;
            return;
        } 
        _inputManager.OnMapClick += OnSpawnObject;
    }

    private void OnDisable()
    {
        _inputManager.OnMapClick -= OnSpawnObject;
    }
}