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

    // Subscribe to edit mode change event
    private void OnEnable()
    {
        DesignerManager.Instance.OnEditTypeChange += HandleEditModeChange;
    }

    // Unsubscribe from edit mode change event
    private void OnDisable()
    {
        DesignerManager.Instance.OnEditTypeChange -= HandleEditModeChange;
    }

    // Handles edit mode change
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

    // Creates a road point at the given position
    public RoadPoint AddPoint(Vector3 pos)
    {
        var container = Instantiate(roadNode, pos, Quaternion.identity);
        var roadPoint = new RoadPoint(pos, container.gameObject);
        container.SetRoadPoint(roadPoint);
        points.Add(roadPoint);
        UpdateRoad();
        return roadPoint;
    }
    
    // Removes a road point
    public void RemovePoint(RoadPoint point)
    {
        point.owner.Delete();
        Destroy(point.gameObject);
        points.Remove(point);
        UpdateRoad();
    }
    
    /// <summary>
    /// Temporary removes a road point<br/>
    /// Triggers when a road point is deleted with right click <br/>
    /// Allows for undoing
    /// </summary>
    public int RemovePointTemporarily(RoadPoint point)
    {
        var i = points.IndexOf(point);
        point.owner.Delete();
        points.Remove(point);
        UpdateRoad();
        return i;
    }
    
    // Adds
    public void AddPoint(int index, RoadPoint point)
    {
        points.Insert(index, point);
        UpdateRoad();
    }

    // Updates the road mesh
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

    // Returns a list of the road points' positions
    private Vector3[] GetPointPositions()
    {
        return points.Select(p => p.position).ToArray();
    }
    
    // Changes the road mesh width
    public void ChangeRoadWidth(float width)
    {
        _roadWidth = width;
        roadMesh.roadWidth = width;
        roadMesh.TriggerUpdate();
    }
    
    // Makes road points smaller
    private void HideRoadPoints()
    {
        foreach (var p in points)
        {
            p.owner.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
    
    // Makes road points normal size
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
