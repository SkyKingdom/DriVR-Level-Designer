using UnityEngine;

public class FirstPersonMode : ModeBase
{
    [SerializeField] private GameObject SceneCamera;
    [SerializeField] private GameObject MainCamera;
    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject Capsule;

    public FirstPersonMode(GameObject sceneCamera, GameObject mainCamera, GameObject canvas, GameObject capsule)
    {
        SceneCamera = sceneCamera;
        MainCamera = mainCamera;
        Canvas = canvas;
        Capsule = capsule;
    }

    public override void OnEnter()
    {
        SceneCamera.SetActive(false);
        MainCamera.SetActive(false);
        Canvas.SetActive(false);
        Capsule.SetActive(true);
    }

    public override void OnExit()
    {
        SceneCamera.SetActive(true);
        MainCamera.SetActive(true);
        Canvas.SetActive(true);
        Capsule.SetActive(false);
    }
}