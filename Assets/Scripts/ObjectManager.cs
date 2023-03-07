using Actions;
using Objects;
using UnityEngine;
using Utilities;

public class ObjectManager : StaticInstance<ObjectManager>
{
    [SerializeField] private ObjectBase prefabToSpawn;
    private InputManager _inputManager;
    
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
        spawnedObject.Select();
        ClearObject();
        return spawnedObject;
    }
    
    public void OnSpawnObject(Vector3 position)
    {
        if (prefabToSpawn == null) return;
        
        var action = new SpawnAction(this, position);
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