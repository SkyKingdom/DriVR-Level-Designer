using Actions;
using Objects;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class SpawnManager : StaticInstance<SpawnManager>
{
    // Selected prefab to spawn
    [SerializeField] private ObjectBase prefabToSpawn;
    public ObjectBase PrefabToSpawn => prefabToSpawn;

    // Object selection handling
    private ObjectBase _clickedObject; // Stores clicked prefab reference to check if should select or deselect
    private Toggle _activeToggle; // Stores the active toggle to deselect if shift is not pressed on spawn

    private bool _isShiftDown; // Checks if shift is pressed to spawn multiple objects

    // Subscribing to events
    private void OnEnable()
    {
        DesignerManager.Instance.InputManager.OnShiftDown += OnShiftDown;
        DesignerManager.Instance.InputManager.OnShiftUp += OnShiftUp;
    }

    #region Spawning Methods

    // Spawns object at given position
    public void SpawnObject(Vector3 pos)
    {
        if (prefabToSpawn == null) return; // If no prefab is selected, return

        // Create action and record it
        var action = new ObjectSpawnAction(prefabToSpawn, pos);
        ActionRecorder.Instance.Record(action);

        // Deselect toggle if shift is not pressed
        if (!_isShiftDown)
        {
            DisableActiveToggle();
        }
    }

    // Spawns path point at given position
    public void SpawnPathPoint(Vector3 pos)
    {
        // Create action and record it
        var action = new PathAddAction(pos);
        ActionRecorder.Instance.Record(action);
    }

    // Spawns road point at given position
    public void AddRoadPoint(Vector3 pos)
    {
        // Create action and record it
        var action = new RoadAddAction(pos);
        ActionRecorder.Instance.Record(action);
    }

    #endregion

    #region Mode Methods
    
    // Selects object to spawn, called from Toggle event
    public void SelectObject(ObjectBase prefab)
    {
        _clickedObject = prefab;
    }
    
    // Selects toggle, called from Toggle event
    public void SelectToggle(Toggle toggle)
    {
        _activeToggle = toggle;
    }

    // Checks if toggle is on or off and sets prefab to spawn accordingly, called from Toggle event
    public void HandleObjectToggle(bool value)
    {
        prefabToSpawn = value ? _clickedObject : null;
        if (!value)
            _activeToggle = null;
    }
    
    // Disables active toggle if there is one
    public void DisableActiveToggle()
    {
        if (!_activeToggle) return;
        _activeToggle.isOn = false;
    }
    
    #endregion
    
    // Handles shift down event
    private void OnShiftDown()
    {
        _isShiftDown = true;
    }
    
    // Handles shift up event
    private void OnShiftUp()
    {
        _isShiftDown = false;
    }
}

