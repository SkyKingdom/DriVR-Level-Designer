using System;
using Actions;
using Interfaces;
using UnityEngine;

public class RoadPointContainer : MonoBehaviour, IEditorInteractable
{
    private RoadPoint _roadPoint;
    public RoadPoint RoadPoint => _roadPoint;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material hoverMaterial;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetRoadPoint(RoadPoint roadPoint)
    {
        _roadPoint = roadPoint;
        _roadPoint.SetOwner(this);
    }

    public bool IsSelected { get; private set;}
    
    private Vector3 _lastPosition;
    
    public void OnPointerEnter()
    {
        if (IsSelected)
            return;
        _renderer.material = hoverMaterial;
    }

    public void OnPointerExit()
    {
        if (IsSelected)
            return;
        
        _renderer.material = defaultMaterial;
    }

    public void OnDrag(Vector3 position)
    {
        transform.position = position;
    }

    public void OnDragRelease()
    {
        if (transform.position == _lastPosition) return;
        
        var position = transform.position;
        
        // Create action
        var action = new RoadDragAction(_lastPosition, position, _roadPoint);
        ActionRecorder.Instance.Record(action);
        
        // Store last position
        _lastPosition = position;
    }

    public void OnRotate(float angle)
    {
        if (SpawnManager.Instance.EditMode != EditMode.Road) return;
        var action = new RoadDeleteAction(_roadPoint);
        ActionRecorder.Instance.Record(action);
    }

    public void OnRotateRelease()
    {
        throw new NotImplementedException();
    }

    public void Select()
    {
        IsSelected = true;
        _renderer.material = hoverMaterial;
        _lastPosition = transform.position;
    }

    public void Deselect()
    {
        IsSelected = false;
        _renderer.material = defaultMaterial;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}