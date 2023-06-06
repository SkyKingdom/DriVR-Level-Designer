using System;using Objects;
using UnityEngine;

public class Node : IDisposable
{
    public ObjectBase Owner { get; private set; }
    public NodeContainer Container { get; private set; }
    public Vector3 Position { get; private set; }
    
    private bool _disposed = false;
    
    private MeshRenderer _renderer;

    public Node(NodeContainer container, ObjectBase owner, Vector3 position)
    {
        Container = container;
        container.Node = this;
        _renderer = container.GetComponent<MeshRenderer>();
        Owner = owner;
        Position = position;
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
        Container.transform.position = position;
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
                if (Container != null)
                {
                    GameObject.Destroy(Container.gameObject);
                }
            }

            // Release any unmanaged resources here.
            _disposed = true;
        }
    }
    
    ~Node()
    {
        Dispose(false);
    }
    
}



