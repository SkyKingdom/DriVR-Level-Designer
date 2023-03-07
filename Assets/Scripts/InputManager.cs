using System;
using UnityEngine;
using UnityEngine.InputSystem;
using User_Interface;
using Utilities;
using Logger = Utilities.Logger;

public class InputManager : StaticInstance<InputManager>
{
    [Header("Ray Casting")]
    [SerializeField] private RectTransform cameraRenderRectTransform;
    [SerializeField] private Camera cam;
    [SerializeField] private OverCanvasCheck overCanvasCheck;
    
    [Header("Input")]
    [SerializeField] private InputActionAsset actionAsset;
    private InputAction _lmb;

    [SerializeField] private Logger logger;
    
    public event Action<Vector3> OnRaycastHit;

    protected override void Awake()
    {
        base.Awake();
        
        _lmb = actionAsset.FindActionMap("Default").FindAction("LMB");
        _lmb.performed += OnPointerClick;
    }

    private void OnEnable()
    {
        _lmb.Enable();
    }

    private void OnDisable()
    {
        _lmb.Disable();
    }

    private void OnPointerClick(InputAction.CallbackContext callbackContext)
    {
        if (!overCanvasCheck.IsOverCanvas) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cameraRenderRectTransform, Mouse.current.position.ReadValue(), null, out var localPoint);
        var rect = cameraRenderRectTransform.rect;
        var pivot = cameraRenderRectTransform.pivot;
        localPoint.x = ( localPoint.x / rect.width ) + pivot.x;
        localPoint.y = ( localPoint.y / rect.height ) + pivot.x;
        var ray = cam.ViewportPointToRay(localPoint);
        if (Physics.Raycast(ray, out var hit))
        {
            OnRaycastHit?.Invoke(hit.point);
            logger.Log($"Raycast hit at {hit.point}", this);
        }
    }
}

