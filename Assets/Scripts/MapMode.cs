using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;

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
        base.OnEnter();
        CameraController.enabled = true;
    }
    
    public override void OnExit()
    {
        base.OnExit();
        CameraController.enabled = false;
    }
    
    public void Search(string query)
    {
        
    }
}