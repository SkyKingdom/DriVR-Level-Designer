using UnityEngine;

public class NodeBase
{
    public Vector3 Position { get; set; }
}

public class ObjectNode : NodeBase
{
    public Vector3 Rotation { get; set; }
    public GameObject Object { get; set; }
}

public class WaypointNode : NodeBase
{
    public float Speed { get; set; }
}


