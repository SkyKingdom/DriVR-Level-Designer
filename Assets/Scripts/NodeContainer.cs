using Actions;
using Interfaces;
using Objects;
using UnityEngine;

public class NodeContainer : MonoBehaviour, IEditorInteractable
{
    public Node Node;

    public Material defaultMaterial;
    public Material selectedMaterial;
    
    private Renderer _renderer;
    
    public bool IsSelected { get; private set; }
    
    private Vector3 _lastPosition;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetNode(Node node)
    {
        Node = node;

        if (node.Owner.IsSelected)
        {
            Highlight();
        }
    }
    
    public void OnPointerEnter()
    {
        if (IsSelected || Node.Owner.Path.Highlighted) return;
        _renderer.material = selectedMaterial;
    }

    public void OnPointerExit()
    {
        if (IsSelected || Node.Owner.Path.Highlighted) return;
        _renderer.material = defaultMaterial;   
    }

    public void OnDrag(Vector3 position)
    {
        transform.position = position;
    }

    public void OnDragRelease()
    {
        if (transform.position == _lastPosition) return;
        
        // Create action
        var action = new NodeDragAction(_lastPosition, transform.position, Node);
        ActionRecorder.Instance.Record(action);
        
        // Store last position
        _lastPosition = transform.position;
    }

    public void OnRotate(float angle)
    {
        if (SpawnManager.Instance.EditMode != EditMode.Path) return;
        
        var action = new PathDeleteAction(Node);
        ActionRecorder.Instance.Record(action);
    }

    public void OnRotateRelease()
    {

    }

    public void Select()
    {
        IsSelected = true;
        _renderer.material = selectedMaterial;
        DesignerManager.Instance.SelectionManager.SelectObject(Node.Owner);
        _lastPosition = transform.position;
        Node.Owner.Path.HighlightPath(this);
    }

    public void Deselect()
    {
        IsSelected = false;
        _renderer.material = defaultMaterial;
        Node.Owner.Path.UnhighlightPath(this);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Highlight()
    {
        _renderer.material = selectedMaterial;
    }

    public void Unhighlight()
    {
        _renderer.material = defaultMaterial;
    }
}