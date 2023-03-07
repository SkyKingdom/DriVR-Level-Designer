using System;
using Actions;
using Objects;
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
    private InputAction _rmb;

    [Header("Dragging")]
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private bool _isLmbDown;
    private bool _isRmbDown;
    [SerializeField] private float dragThreshold = 1f;
    [SerializeField] private Transform dragObject;
    [SerializeField] private Vector3 objectInitialPosition;
    [SerializeField] private Vector3 objectInitialRotation;
    [SerializeField] [Range(0f, 5f)] private float rotationSpeed = 1f;

    [SerializeField] private Logger logger;
    
    public event Action<Vector3> OnMapClick;

    protected override void Awake()
    {
        base.Awake();
        
        _lmb = actionAsset.FindActionMap("Default").FindAction("LMB");
        _rmb = actionAsset.FindActionMap("Default").FindAction("RMB");
        _lmb.started += OnLmbDown;
        _lmb.canceled += OnLmbRelease;
        _rmb.started += OnRmbDown;
        _rmb.canceled += OnRmbRelease;
    }
    
    private void OnEnable()
    {
        _lmb.Enable();
        _rmb.Enable();
    }

    private void OnDisable()
    {
        _lmb.Disable();
        _rmb.Disable();
    }

    private void Update()
    {
        if (_isLmbDown && overCanvasCheck.IsOverCanvas && dragObject)
        {
            var currentPos = Mouse.current.position.ReadValue();
            var distance = Vector2.Distance(_startPosition, currentPos);
            if (distance > dragThreshold)
            {
                dragObject.transform.position = GetRaycastPosition(currentPos);
            }
        }
        
        if (_isRmbDown && overCanvasCheck.IsOverCanvas && dragObject)
        {
            var currentPos = Mouse.current.position.ReadValue();
            var distance = _startPosition.x - currentPos.x;
            if (Mathf.Abs(distance) > dragThreshold)
            {
                if (distance > 0)
                {
                    distance = rotationSpeed;
                } else if (distance < 0)
                {
                    distance = -rotationSpeed;
                }
                dragObject.transform.Rotate(0, distance, 0);
            }

            _startPosition = currentPos;
        }
    }

    private void OnLmbDown (InputAction.CallbackContext callbackContext)
    {
        if (!overCanvasCheck.IsOverCanvas) return;
        if (_isRmbDown) return;
        _startPosition = Mouse.current.position.ReadValue();
        _isLmbDown = true;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cameraRenderRectTransform, Mouse.current.position.ReadValue(), null, out var localPoint);
        var rect = cameraRenderRectTransform.rect;
        var pivot = cameraRenderRectTransform.pivot;
        localPoint.x = ( localPoint.x / rect.width ) + pivot.x;
        localPoint.y = ( localPoint.y / rect.height ) + pivot.x;
        var ray = cam.ViewportPointToRay(localPoint);
        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.transform.TryGetComponent(out ObjectBase objectBase))
            {
                dragObject = objectBase.transform;
                objectInitialPosition = objectBase.transform.position;
            }
        }
    }

    private void OnLmbRelease(InputAction.CallbackContext callbackContext)
    {
        if (!overCanvasCheck.IsOverCanvas)
        {
            if (dragObject)
            {
                var dragAction = new DragAction(objectInitialPosition, dragObject.position, dragObject);
                ActionRecorder.Instance.Record(dragAction);
                dragObject = null;
            }
            return;
        }
        
        _endPosition = Mouse.current.position.ReadValue();
        var distance = Vector2.Distance(_startPosition, _endPosition);
        var isDrag = distance > dragThreshold;
        _isLmbDown = false;
       
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cameraRenderRectTransform, Mouse.current.position.ReadValue(), null, out var localPoint);
        var rect = cameraRenderRectTransform.rect;
        var pivot = cameraRenderRectTransform.pivot;
        localPoint.x = ( localPoint.x / rect.width ) + pivot.x;
        localPoint.y = ( localPoint.y / rect.height ) + pivot.x;
        var ray = cam.ViewportPointToRay(localPoint);
        if (Physics.Raycast(ray, out var hit))
        {
            if (isDrag)
            {
                var dragAction = new DragAction(objectInitialPosition, hit.point, dragObject);
                ActionRecorder.Instance.Record(dragAction);
            }
            else
            {
                OnMapClick?.Invoke(hit.point);
                logger.Log($"Raycast click at {hit.point}", this);
            }
        }
        dragObject = null;
    }
    
    private void OnRmbDown(InputAction.CallbackContext callbackContext)
    {
        logger.Log("RMB", this);
        if (!overCanvasCheck.IsOverCanvas) return;
        if (_isLmbDown) return;
        _startPosition = Mouse.current.position.ReadValue();
        _isRmbDown = true;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cameraRenderRectTransform, Mouse.current.position.ReadValue(), null, out var localPoint);
        var rect = cameraRenderRectTransform.rect;
        var pivot = cameraRenderRectTransform.pivot;
        localPoint.x = ( localPoint.x / rect.width ) + pivot.x;
        localPoint.y = ( localPoint.y / rect.height ) + pivot.x;
        var ray = cam.ViewportPointToRay(localPoint);
        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.transform.TryGetComponent(out ObjectBase objectBase))
            {
                dragObject = objectBase.transform;
                objectInitialRotation = objectBase.transform.rotation.eulerAngles;
            }
        }
    }
    
    private void OnRmbRelease(InputAction.CallbackContext callbackContext)
    {
        logger.Log("RMB release", this);
        _isRmbDown = false;
        if (dragObject)
        {
            dragObject = null;
        }
    }
    
    private Vector3 GetRaycastPosition(Vector2 mousePosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cameraRenderRectTransform, mousePosition, null, out var localPoint);
        var rect = cameraRenderRectTransform.rect;
        var pivot = cameraRenderRectTransform.pivot;
        localPoint.x = ( localPoint.x / rect.width ) + pivot.x;
        localPoint.y = ( localPoint.y / rect.height ) + pivot.x;
        var ray = cam.ViewportPointToRay(localPoint);
        if (Physics.Raycast(ray, out var hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}

