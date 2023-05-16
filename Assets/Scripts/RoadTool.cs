using System;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using PathCreation.Examples;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

public class RoadTool : StaticInstance<RoadTool>
{
    [SerializeField] private PathCreator roadPrefab;
    [SerializeField] private RoadPointContainer roadNode;

    private PathCreator road;
    private RoadMeshCreator roadMesh;
    private List<RoadPoint> points = new();
    [SerializeField] private float tilingMultiplier = 7f;

    public RoadPoint AddPoint(Vector3 pos)
    {
        var container = Instantiate(roadNode, pos, Quaternion.identity);
        var roadPoint = new RoadPoint(pos, container.gameObject);
        container.SetRoadPoint(roadPoint);
        points.Add(roadPoint);
        UpdateRoad();
        return roadPoint;
    }
    
    public void RemovePoint(RoadPoint point)
    {
        Destroy(point.gameObject);
        points.Remove(point);
        UpdateRoad();
    }

    public void UpdateRoad()
    {
        if (!road)
        {
            road = Instantiate(roadPrefab);
            roadMesh = road.GetComponent<RoadMeshCreator>();
            roadMesh.TriggerUpdate();
        }

        road.gameObject.SetActive(points.Count >= 2);
        if (points.Count < 2)
            return;
        var positions = GetPointPositions();
        BezierPath bezierPath = new BezierPath(positions, false, PathSpace.xz);
        road.bezierPath = bezierPath;

        if (road.gameObject.activeSelf)
        {
            road.TriggerPathUpdate();
            roadMesh.textureTiling = positions.Length * tilingMultiplier;
            roadMesh.TriggerUpdate();
        }
    }

    private Vector3[] GetPointPositions()
    {
        return points.Select(p => p.position).ToArray();
    }
}


[Serializable]
public class RoadPoint
{
    public GameObject gameObject;
    public Vector3 position;
    public RoadPointContainer owner;
    
    public RoadPoint(Vector3 position, GameObject gameObject)
    {
        this.position = position;
        this.gameObject = gameObject;
    }
    
    public void SetOwner(RoadPointContainer o)
    {
        owner = o;
    }
}
