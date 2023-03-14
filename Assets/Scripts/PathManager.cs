using Objects;
using UnityEngine;
using Utilities;

public class PathManager : StaticInstance<PathManager>
{
    private PathObject _selectedObject;
    public GameObject pathPointPrefab;
    private LineRenderer _lineRenderer;

    protected override void Awake()
    {
        base.Awake();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        InputManager.Instance.OnPathClick += HandlePathClick;
    }
    
    public void SelectObject(PathObject obj) => _selectedObject = obj;
    
    public void DeselectObject() => _selectedObject = null;
    
    private void HandlePathClick(Vector3 point)
    {
        var obj = Instantiate(pathPointPrefab, point, Quaternion.identity);
        var Node = new Node(obj, point);
        _selectedObject.AddPathPoint(Node);
        _lineRenderer.positionCount++;
        var index = _selectedObject.PathPoints.Count - 1;
        _lineRenderer.SetPosition(index, point);
    }
    
}