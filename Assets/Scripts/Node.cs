using System;using Objects;
using UnityEngine;

public class Node : IDisposable
{
    public ObjectBase Owner { get; private set; }
    public GameObject GameObject { get; private set; }
    public Vector3 Position { get; private set; }
    
    private bool _disposed = false;
    
    private MeshRenderer _renderer;

    public bool Selected { get; private set; } = false;

    public Node(GameObject gameObject, ObjectBase owner, Vector3 position)
    {
        GameObject = gameObject;
        _renderer = gameObject.GetComponent<MeshRenderer>();
        Owner = owner;
        Position = position;
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
        GameObject.transform.position = position;
        OnPositionChange();
    }

    private void OnPositionChange()
    {
        Owner.Path.RepositionPathPoint(this);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Release any managed resources here.
                // In this case, we don't have any managed resources to release.
                if (GameObject != null)
                {
                    GameObject.Destroy();
                }
            }

            // Release any unmanaged resources here.
            _disposed = true;
        }
    }
    
    public void Select()
    {
        Selected = true;
        _renderer.material.color = Color.red;
    }
    
    public void Deselect()
    {
        Selected = false;
        _renderer.material.color = Color.green;
    }

    ~Node()
    {
        Dispose(false);
    }
    
}



