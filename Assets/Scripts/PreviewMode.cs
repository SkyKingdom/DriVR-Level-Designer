using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewMode : MonoBehaviour
{
    [SerializeField] private GameObject sceneCamera;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject capsule;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchToPreviewMode();
        }
    }

    private void SwitchToPreviewMode()
    {
        capsule.SetActive(!capsule.activeSelf);
        sceneCamera.SetActive(!sceneCamera.activeSelf);
        mainCamera.SetActive(!mainCamera.activeSelf);
        canvas.SetActive(!canvas.activeSelf);
    }
}
