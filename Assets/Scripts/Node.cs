using System;using Objects;
using UnityEngine;

public class Node : IDisposable
{
    private ObjectBase Owner { get; set; }
    public GameObject GameObject { get; private set; }
    public Vector3 Position { get; private set; }
    
    private bool _disposed = false;
    
    public Node(GameObject gameObject, ObjectBase owner, Vector3 position)
    {
        GameObject = gameObject;
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
            }

            // Release any unmanaged resources here.
            if (GameObject != null)
            {
                GameObject.Destroy();
            }

            _disposed = true;
        }
    }

    ~Node()
    {
        Dispose(false);
    }
    
}



