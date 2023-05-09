using System;
using System.Collections.Generic;
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
        Destroy(point.GameObject);
        var index = points.FindIndex(p => p == point);
        points.Remove(point);
        road.bezierPath.DeleteSegment(index);
        UpdateRoad();
    }

    private void UpdateRoad()
    {
        if (!road)
        {
            road = Instantiate(roadPrefab);
            roadMesh = road.GetComponent<RoadMeshCreator>();
            roadMesh.TriggerUpdate();
        }

        road.gameObject.SetActive(points.Count >= 2);
        for (int i = 0; i < points.Count; i++)
        {
            if (i < road.bezierPath.NumPoints)
            {
                road.bezierPath.SetPoint(i, points[i].Position);
                continue;
            }
            road.bezierPath.AddSegmentToEnd(points[i].Position);
        }

        if (road.gameObject.activeSelf)
        {
            road.TriggerPathUpdate();
            roadMesh.TriggerUpdate();
        }
    }
}


[Serializable]
public class RoadPoint
{
    public GameObject GameObject;
    public Vector3 Position;
    
    public RoadPoint(Vector3 position, GameObject gameObject)
    {
        Position = position;
        GameObject = gameObject;
    }

}
