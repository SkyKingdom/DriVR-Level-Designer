using System;
using Actions;
using Interfaces;
using UnityEngine;

public class RoadPointContainer : MonoBehaviour, IEditorInteractable
{
    
    public RoadPoint RoadPoint { get; private set; }

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material hoverMaterial;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetRoadPoint(RoadPoint roadPoint)
    {
        RoadPoint = roadPoint;
        RoadPoint.SetOwner(this);
    }

    public event Action OnObjectDeleted;
    public bool IsSelected { get; private set;}
    
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
    
    public void Select()
    {
        IsSelected = true;
        _renderer.material = hoverMaterial;
        DesignerManager.Instance.SelectionManager.SelectRoadPoint(this);
    }

    public void Deselect()
    {
        IsSelected = false;
        _renderer.material = defaultMaterial;
        DesignerManager.Instance.SelectionManager.DeselectRoadPoint();
    }

    public Transform GetTransform()
    {
        return transform;
    }
    
    public void Delete()
    {
        OnObjectDeleted?.Invoke();
        _renderer.material = defaultMaterial;
    }
}