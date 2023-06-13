using System;
using Actions;
using Managers;
using Objects;
using PathCreation;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class PathManager : StaticInstance<PathManager>
{
    #region Dependencies

    private SelectionManager _selectionManager;

    #endregion
    
    private ObjectBase _selectedObject;
    public NodeContainer pathPointPrefab;
    
    [SerializeField] private Button stopEditingButton;
    [SerializeField] private PathCreator pathPrefab;

    [SerializeField] private float snappingDistanceThreshold = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        _selectionManager = DesignerManager.Instance.SelectionManager;
    }

    private void OnEnable()
    {
        _selectionManager.OnObjectSelected += SelectObject;
        _selectionManager.OnObjectDeselected += DeselectObject;
    }
    
    private void OnDisable()
    {
        _selectionManager.OnObjectSelected -= SelectObject;
        _selectionManager.OnObjectDeselected -= DeselectObject;
    }

    private void SelectObject(ObjectBase obj) => _selectedObject = obj;

    private void DeselectObject() => _selectedObject = null;

    public void HandlePathPointSpawn(Vector3 pos)
    {
        var point = ShouldSnapRoad(pos);
        
        if (point != null)
        {
            pos = point.Value.point;
        }
        
        var cont = Instantiate(pathPointPrefab, pos, Quaternion.identity);
        var node = new Node(cont, _selectedObject, pos);
        cont.SetNode(node);
        var action = new NodeAddAction(node, _selectedObject);
        ActionRecorder.Instance.Record(action);
    }

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

    public PathCreator GetNewPath()
    {
        return Instantiate(pathPrefab);
    }
    
    public void UpdatePathSnappingThreshold(float value)
    {
        snappingDistanceThreshold = value;
    }
    
}