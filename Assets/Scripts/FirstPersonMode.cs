using UnityEngine;

public class FirstPersonMode : ModeBase
{
    public GameObject SceneCamera {get; private set;}
    public GameObject MainCamera {get; private set;}
    public GameObject Canvas {get; private set;}
    public GameObject Capsule {get; private set;}

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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnExit()
    {
        SceneCamera.SetActive(true);
        MainCamera.SetActive(true);
        Canvas.SetActive(true);
        Capsule.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}