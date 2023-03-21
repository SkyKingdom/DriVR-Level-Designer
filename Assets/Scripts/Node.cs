using Objects;
using UnityEngine;

public class Node
{
    public PathObject Owner { get; private set; }
    public GameObject GameObject { get; private set; }
    public Vector3 Position { get; private set; }
    
    public Node(GameObject gameObject, Vector3 position,PathObject owner)
    {
        GameObject = gameObject;
        Position = position;
        Owner = owner;
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
        GameObject.transform.position = position;
    }
    
    public void ReportPositionChange()
    {
        Owner.HandleNodePositionChange(this);
    }
    
}



