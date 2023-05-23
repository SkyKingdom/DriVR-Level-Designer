using Actions;
using Interfaces;
using UnityEngine;

public class NodeContainer : MonoBehaviour, IEditorInteractable
{
    public Node node;

    public Material defaultMaterial;
    public Material selectedMaterial;
    
    private Renderer _renderer;
    
    public bool IsSelected { get; private set; }
    
    private Vector3 _lastPosition;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
    
    public void OnPointerEnter()
    {
        if (IsSelected || node.Selected) return;
        _renderer.material = selectedMaterial;
    }

    public void OnPointerExit()
    {
        if (IsSelected || node.Selected) return;
        _renderer.material = defaultMaterial;   
    }

    public void OnDrag(Vector3 position)
    {
        if (SpawnManager.Instance.EditType == EditType.Path)
            transform.position = position;
    }

    public void OnDragRelease()
    {
        if (transform.position == _lastPosition) return;
        
        // Create action
        var action = new NodeDragAction(_lastPosition, transform.position, node);
        ActionRecorder.Instance.Record(action);
        
        // Store last position
        _lastPosition = transform.position;
    }

    public void OnRotate(float angle)
    {
        if (SpawnManager.Instance.EditType != EditType.Path) return;
        
        var action = new PathDeleteAction(node);
        ActionRecorder.Instance.Record(action);
    }

    public void OnRotateRelease()
    {

    }

    public void Select()
    {
        IsSelected = true;
        _renderer.material = selectedMaterial;
        
        _lastPosition = transform.position;
        node.Owner.Path.Select();
    }

    public void Deselect()
    {
        IsSelected = false;
        _renderer.material = defaultMaterial;
        node.Owner.Path.Deselect();
    }
}