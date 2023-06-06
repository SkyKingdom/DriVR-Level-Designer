using Mapbox.Unity.Map;

public class MapMode : ModeBase
{
    private readonly CameraController _cameraController;
    private readonly AbstractMap _map;
    public MapMode(AbstractMap map, CameraController cameraController)
    {
        _map = map;
        _cameraController = cameraController;
    }
    public override void OnEnter()
    {
        // Enable camera movement & update map
        _cameraController.enabled = true;
        _map.UpdateMap();
        _map.ForceUpdateColliders();
    }
    
    public override void OnExit()
    {
        // Disable camera movement
        _cameraController.enabled = false;
    }
}

