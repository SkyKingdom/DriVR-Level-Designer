using System;using Objects;
using UnityEngine;

public class PathPoint : IDisposable
{
    public ObjectBase Owner { get; }
    public PathPointContainer Container { get; }
    public Vector3 Position { get; private set; }
    
    private bool _disposed;
    
    public PathPoint(PathPointContainer container, ObjectBase owner, Vector3 position)
    {
        Container = container;
        container.PathPoint = this;
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

    private void Dispose(bool disposing)
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
    
    ~PathPoint()
    {
        Dispose(false);
    }
    
}



