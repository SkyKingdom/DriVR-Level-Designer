using Actions;
using Objects;
using UnityEngine;
using Utilities;

public class ObjectManager : StaticInstance<ObjectManager>
{
    [SerializeField] private ObjectBase _prefabToSpawn;
    private InputManager _inputManager;
    
    public void SelectObject(ObjectBase prefab)
    {
        _prefabToSpawn = prefab;
    }
    
    public void ClearObject()
    {
        _prefabToSpawn = null;
    }

    public ObjectBase Spawn(Vector3 position)
    {
        var spawnedObject = Instantiate(_prefabToSpawn, position, Quaternion.identity);
        spawnedObject.Select();
        ClearObject();
        return spawnedObject;
    }
    
    public void OnSpawnObject(Vector3 position)
    {
        if (_prefabToSpawn == null) return;
        
        var action = new SpawnAction(this, position);
        ActionRecorder.Instance.Record(action);
    }

    private void OnEnable()
    {
        if (_inputManager == null)
        {
            _inputManager = FindObjectOfType<InputManager>();
            if (_inputManager)
                _inputManager.OnRaycastHit += OnSpawnObject;
            return;
        } 
        _inputManager.OnRaycastHit += OnSpawnObject;
    }

    private void OnDisable()
    {
        _inputManager.OnRaycastHit -= OnSpawnObject;
    }
}