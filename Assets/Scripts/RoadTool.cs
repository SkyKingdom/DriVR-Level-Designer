using System;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class RoadTool : MonoBehaviour
{
    public Spline spline;
    public SplineSmoother smoother;
    public List<SplineNode> points = new();

    private void Start()
    {
        spline.nodes.Clear();
    }

    public void AddPoint(SplineNode node)
    {
        Debug.Log("ADDING NODE");
        points.Add(node);
        spline.AddNode(node);
        if (points.Count < 2) return;
        spline.gameObject.SetActive(true);
        smoother.SmoothAll();
    }

    public void RemovePoint(SplineNode node)
    {
        spline.RemoveNode(node);
        points.Remove(node);
        
        if (points.Count < 2)
        {
            spline.gameObject.SetActive(false);
            return;
        }
        spline.RefreshCurves();
        smoother.SmoothAll();
    }
}
