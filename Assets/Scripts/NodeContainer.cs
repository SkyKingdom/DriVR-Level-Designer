using Interfaces;
using UnityEngine;

public class NodeContainer : MonoBehaviour, IEditorInteractable
{
    public Node node;

    public bool IsSelected { get; }
    public void OnPointerEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit()
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    public void Deselect()
    {
        throw new System.NotImplementedException();
    }
}