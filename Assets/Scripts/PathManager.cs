using Actions;
using Objects;
using PathCreation;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class PathManager : StaticInstance<PathManager>
{
    private ObjectBase _selectedObject;
    public NodeContainer pathPointPrefab;

    [SerializeField] private Material selected;
    [SerializeField] private Material deselected;
    
    public Material Selected => selected;

    public Material Deselected => deselected;

    [SerializeField] private Button stopEditingButton;
    [SerializeField] private PathCreator pathPrefab;

    [SerializeField] private float snappingDistanceThreshold = 0.5f;
    
    public void SelectObject(ObjectBase obj) => _selectedObject = obj;

    public void DeselectObject()
    {
        _selectedObject = null;
        
        if (SpawnManager.Instance.EditMode == EditMode.Path)
            stopEditingButton.onClick.Invoke();
    }

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
    
}