using System;
using Interfaces;
using UnityEngine;

public class RoadPointContainer : MonoBehaviour, IEditorInteractable
{
    private RoadPoint _roadPoint;

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
    }

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

    public void OnDrag(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public void OnDragRelease()
    {
        throw new System.NotImplementedException();
    }

    public void OnRotate(float angle)
    {
        throw new System.NotImplementedException();
    }

    public void OnRotateRelease()
    {
        throw new System.NotImplementedException();
    }

    public void Select()
    {
        IsSelected = true;
        _renderer.material = hoverMaterial;
    }

    public void Deselect()
    {
        IsSelected = false;
        _renderer.material = defaultMaterial;
    }
}