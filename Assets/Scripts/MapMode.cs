using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using UnityEngine;

public class MapMode : ModeBase
{
    public CameraController CameraController;
    public AbstractMap Map;
    public GameObject Plane;
    
    public MapMode(AbstractMap map, CameraController cameraController, GameObject plane)
    {
        Map = map;
        CameraController = cameraController;
        Plane = plane;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        CameraController.enabled = true;
        GetMapData();
    }
    
    public override void OnExit()
    {
        base.OnExit();
        CameraController.enabled = false;
    }
    
    public void ToggleMap(bool value)
    {
        Plane.SetActive(!value);
        Map.gameObject.SetActive(value);
        Map.Terrain.EnableCollider(value);
    }

    public MapData GetMapData()
    {
        var mapData = new MapData();
        mapData.Zoom = Map.Zoom;
        mapData.CenterX = Map.CenterLatitudeLongitude.x;
        mapData.CenterY = Map.CenterLatitudeLongitude.y;

        return mapData;
    }
}

public struct MapData
{
    public float Zoom;
    public double CenterX;
    public double CenterY;
}