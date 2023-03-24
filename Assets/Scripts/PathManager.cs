using Actions;
using Objects;
using UnityEngine;
using Utilities;

public class PathManager : StaticInstance<PathManager>
{
    private ObjectBase _selectedObject;
    public NodeContainer pathPointPrefab;

    [SerializeField] private Material selected;
    [SerializeField] private Material deselected;

    public Material Selected => selected;

    public Material Deselected => deselected;

    private void OnEnable()
    {
        InputManager.Instance.OnPathClick += HandlePathClick;
    }
    
    public void SelectObject(ObjectBase obj) => _selectedObject = obj;

    public void DeselectObject() => _selectedObject = null;

    private void HandlePathClick(Vector3 point)
    {
        var cont = Instantiate(pathPointPrefab, point, Quaternion.identity);
        var node = new Node(cont.gameObject, _selectedObject, point);
        cont.node = node;
        var action = new NodeAddAction(node, _selectedObject);
        ActionRecorder.Instance.Record(action);
    }

    
}