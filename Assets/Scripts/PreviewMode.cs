using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreviewMode : MonoBehaviour
{
    private bool IsPreviewMode => LevelGeneratorManager.Instance.Mode == Mode.FirstPerson;
    [SerializeField] private InputActionReference escapeAction;

    private void OnEnable()
    {
        escapeAction.action.Enable();
        escapeAction.action.performed += OnEscapePressed;
    }

    private void OnDisable()
    {
        escapeAction.action.Disable();
        escapeAction.action.performed -= OnEscapePressed;
    }

    private void OnEscapePressed(InputAction.CallbackContext obj)
    {
        if (!IsPreviewMode)
            return;
        TogglePreview();
    }


    public void TogglePreview()
    {
        LevelGeneratorManager.Instance.SetMode(IsPreviewMode ? 1 : 3);
    }

}
