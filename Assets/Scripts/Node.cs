using UnityEngine;

public class Node
{
    public GameObject GameObject { get; private set; }
    public Vector3 Position { get; private set; }
    
    public Node(GameObject gameObject, Vector3 position)
    {
        GameObject = gameObject;
        Position = position;
    }
    
    public void SetPosition(Vector3 position)
    {
        Position = position;
        GameObject.transform.position = position;
    }
    
}



