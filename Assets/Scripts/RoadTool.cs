using System;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;
using Utilities;

public class RoadTool : StaticInstance<RoadTool>
{
    // Road prefab
    [Header("Prefab References") ,SerializeField] private PathCreator roadPrefab;
    // Road point prefab
    [SerializeField] private RoadPointContainer roadNode;

    // Reference to road spline
    private PathCreator road;
    public PathCreator Road => road;
    
    // Reference to road mesh
    private RoadMeshCreator roadMesh;
    
    // List of road points
    private List<RoadPoint> points = new();
    
    // Road material tiling multiplier
    [Header("Settings"), SerializeField] private float tilingMultiplier = 7f;
    
    // Number of points to use for road mesh
    [SerializeField] private int numPointsForMesh = 3;
    
    // Returns a list of the road points' positions
    public Vector3[] RoadPoints => GetPointPositions();
    
    // returns if road mesh is active
    public bool HasRoad => points.Count >= 3;
    
    private float _roadWidth = 1f;

    private void OnEnable()
    {
        DesignerManager.Instance.OnEditTypeChange += HandleEditModeChange;
    }

    private void OnDisable()
    {
        DesignerManager.Instance.OnEditTypeChange -= HandleEditModeChange;
    }

    private void HandleEditModeChange(EditMode oldValue, EditMode value)
    {
        if (oldValue == EditMode.Road)
        {
            HideRoadPoints();
        }
        
        if (value == EditMode.Road)
        {
            ShowRoadPoints();
        }
    }

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
        point.owner.Delete();
        Destroy(point.gameObject);
        points.Remove(point);
        UpdateRoad();
    }
    
    public int RemovePointTemporarily(RoadPoint point)
    {
        var i = points.IndexOf(point);
        point.owner.Delete();
        points.Remove(point);
        UpdateRoad();
        return i;
    }
    
    public void AddPoint(int index, RoadPoint point)
    {
        points.Insert(index, point);
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

        road.gameObject.SetActive(points.Count >= numPointsForMesh);
        if (points.Count < numPointsForMesh)
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
    
    public void ChangeRoadWidth(float width)
    {
        _roadWidth = width;
        roadMesh.roadWidth = width;
        roadMesh.TriggerUpdate();
    }
    
    private void HideRoadPoints()
    {
        foreach (var p in points)
        {
            p.owner.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
    
    private void ShowRoadPoints()
    {
        foreach (var p in points)
        {
            p.owner.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }
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

    public void SetPosition(Vector3 pos)
    {
        position = pos;
        owner.transform.position = pos;
    }
}
