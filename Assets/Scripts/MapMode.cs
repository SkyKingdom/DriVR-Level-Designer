using Mapbox.Unity.Map;
using UnityEngine;

public class MapMode : ModeBase
{
    public CameraController CameraController;
    public AbstractMap Map;
    public MapMode(AbstractMap map, CameraController cameraController)
    {
        Map = map;
        CameraController = cameraController;
    }
    public override void OnEnter()
    {
        CameraController.enabled = true;
        Map.UpdateMap();
        Map.ForceUpdateColliders();
    }
    
    public override void OnExit()
    {
        CameraController.enabled = false;
    }
}

