using Objects;
using UnityEngine;
using Utilities;

public class PathManager : StaticInstance<PathManager>
{
    private PathObject _selectedObject;
    public GameObject pathPointPrefab;

    [SerializeField] private Material selected;
    [SerializeField] private Material deselected;

    public Material Selected => selected;

    public Material Deselected => deselected;

    private void OnEnable()
    {
        InputManager.Instance.OnPathClick += HandlePathClick;
    }
    
    public void SelectObject(PathObject obj) => _selectedObject = obj;

    public void DeselectObject() => _selectedObject = null;

    private void HandlePathClick(Vector3 point)
    {
        var obj = Instantiate(pathPointPrefab, point, Quaternion.identity);
        var Node = new Node(obj, point, _selectedObject);
        _selectedObject.AddPathPoint(Node);
    }

    
}