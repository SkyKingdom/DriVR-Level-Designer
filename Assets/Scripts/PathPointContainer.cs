using System;
using Interfaces;
using UnityEngine;

public class PathPointContainer : MonoBehaviour, IEditorInteractable
{
    // Reference to path point data
    public PathPoint PathPoint;

    // References to the materials
    public Material defaultMaterial;
    public Material selectedMaterial;
    
    private Renderer _renderer;
    public event Action OnObjectDeleted;
    public bool IsSelected { get; private set; }
    

    // Get reference to renderer
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Set path point data reference
    public void SetNode(PathPoint pathPoint)
    {
        PathPoint = pathPoint;

        if (pathPoint.Owner.IsSelected)
        {
            Highlight();
        }
    }
    
    // Change material on hover
    public void OnPointerEnter()
    {
        if (IsSelected || PathPoint.Owner.Path.Highlighted) return;
        _renderer.material = selectedMaterial;
    }

    // Change material on stop hovering
    public void OnPointerExit()
    {
        if (IsSelected || PathPoint.Owner.Path.Highlighted) return;
        _renderer.material = defaultMaterial;   
    }
    
    public void Select()
    {
        IsSelected = true;
        _renderer.material = selectedMaterial;
        DesignerManager.Instance.SelectionManager.SelectPathPoint(this);
        PathPoint.Owner.Path.HighlightPath(this);
    }

    public void Deselect()
    {
        IsSelected = false;
        _renderer.material = defaultMaterial;
        DesignerManager.Instance.SelectionManager.DeselectPathPoint();
        PathPoint.Owner.Path.UnhighlightPath(this);
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
    
    public void Delete()
    {
        OnObjectDeleted?.Invoke();
        Unhighlight();
    }
}