using Actions;
using Managers;
using Objects;
using PathCreation;
using UnityEngine;
using Utilities;

public class PathManager : StaticInstance<PathManager>
{
    #region Dependencies

    private SelectionManager _selectionManager;

    #endregion
    
    // Reference to selected object
    private ObjectBase _selectedObject;
    
    // Reference to path point prefab
    public PathPointContainer pathPointPrefab;

    // Reference to path prefab
    [SerializeField] private PathCreator pathPrefab;

    // Snapping distance threshold path points and roads
    [SerializeField] private float snappingDistanceThreshold = 0.5f;

    // Get dependency references
    protected override void Awake()
    {
        base.Awake();
        _selectionManager = DesignerManager.Instance.SelectionManager;
    }

    // Subscribe to events
    private void OnEnable()
    {
        _selectionManager.OnObjectSelected += SelectObject;
        _selectionManager.OnObjectDeselected += DeselectObject;
    }
    
    // Unsubscribe from events
    private void OnDisable()
    {
        _selectionManager.OnObjectSelected -= SelectObject;
        _selectionManager.OnObjectDeselected -= DeselectObject;
    }
    
    private void SelectObject(ObjectBase obj) => _selectedObject = obj;

    private void DeselectObject() => _selectedObject = null;

    // Instantiate path point
    public PathPoint InstantiatePathPoint(Vector3 pos)
    {
        var point = ShouldSnapRoad(pos);
        
        if (point != null)
        {
            pos = point.Value.point;
        }
        
        var cont = Instantiate(pathPointPrefab, pos, Quaternion.identity);
        var pathPoint = new PathPoint(cont, _selectedObject, pos);
        cont.SetNode(pathPoint);

        return pathPoint;
    }

    // Check if path point should snap to road
    private ClosePointData? ShouldSnapRoad(Vector3 pos)
    {
        if (!RoadTool.Instance.HasRoad) return null;
        var closestPoint = RoadTool.Instance.Road.bezierPath.GetClosestPointOnPath(pos, 0.05f, 50);
        if (closestPoint.distance < snappingDistanceThreshold)
        {
            return closestPoint;
        }
        
        return null;
    }

    // Instantiate path
    public PathCreator GetNewPath()
    {
        return Instantiate(pathPrefab);
    }
    
    // Update snapping distance threshold
    public void UpdatePathSnappingThreshold(float value)
    {
        snappingDistanceThreshold = value;
    }
}