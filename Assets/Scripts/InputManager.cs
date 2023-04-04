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
    private bool _isLmbDown;
    private bool _isRmbDown;
    private bool _editingPath;
    private bool _inEditMode;
    private Vector2 _mouseStartPosition;
    private Vector2 _mouseEndPosition;
    
    [Header("Object Editing")]
    [SerializeField] private float dragThreshold = 1f;
    [SerializeField] [Range(0f, 5f)] private float rotationSpeed = 1f;
    [SerializeField] private ObjectBase dragObject;
    [SerializeField] private Vector3 objectInitialPosition;
    [SerializeField] private Vector3 objectInitialRotation;
    
    [Header("Path Editing")]
    [SerializeField] private NodeContainer dragNode;
    [Header("Debugging")]
    [SerializeField] private Logger logger;

    
    public event Action<Vector3> OnMapClick;
    public event Action<ObjectBase> OnObjectSelect;
    public event Action OnObjectDeselect;

    public event Action<Vector3> OnPathClick;

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

    private void Start()
    {
        LevelGeneratorManager.Instance.OnModeChange += HandleModeChange;
    }

    private void HandleModeChange(Mode mode)
    {
        if (mode == Mode.Edit)
            _inEditMode = true;
        else
        {
            _inEditMode = false;
            dragObject = null;
            OnObjectDeselect?.Invoke();
        }
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

    public void TogglePathEdit()
    {
        _editingPath = !_editingPath;

        switch (_editingPath)
        {
            case true:
                _lmb.started -= OnLmbDown;
                _lmb.canceled -= OnLmbRelease;
                _rmb.started -= OnRmbDown;
                _rmb.canceled -= OnRmbRelease;
                
                _lmb.started += OnPathLmbDown;
                _lmb.canceled += OnPathLmbRelease;
                break;
            case false:
                _lmb.started -= OnPathLmbDown;
                _lmb.canceled -= OnPathLmbRelease;
                
                _lmb.started += OnLmbDown;
                _lmb.canceled += OnLmbRelease;
                _rmb.started += OnRmbDown;
                _rmb.canceled += OnRmbRelease;
                break;
        }
    }

    private void Update()
    {
        if (!_inEditMode) return;
        if (!overCanvasCheck.IsOverCanvas) return; // Check if mouse is over canvas/map

        var currentPos = Mouse.current.position.ReadValue(); // Get current mouse position
        ObjectDragUpdate(currentPos);
        NodeDragUpdate(currentPos);
    }

    private void ObjectDragUpdate(Vector2 mousePos)
    {
        if (!dragObject) return; // Check if there is an object to drag
        // Handle left mouse button
        if (_isLmbDown)
        {
            // check if mouse has moved more than drag threshold
            var distance = Vector2.Distance(_mouseStartPosition, mousePos);
            if (distance < dragThreshold) return;
            
            // if mouse has moved more than drag threshold, set object position to mouse position
            // keep the y position of the object the same;
            var raycastPosition = GetRaycastPosition(mousePos);
            raycastPosition.y = 0f;
            dragObject.transform.position = raycastPosition;

            return;
        }
        
        // Handle right mouse button
        if (_isRmbDown)
        {
            // Check distance on the x axis
            var distance = _mouseStartPosition.x - mousePos.x;
            if (Mathf.Abs(distance) > dragThreshold)
            {
                // Change rotation direction based on distance direction
                distance = distance switch
                {
                    > 0 => rotationSpeed,
                    < 0 => -rotationSpeed,
                    _ => distance
                };
                // Rotate object
                dragObject.transform.Rotate(0, distance, 0);
            }
            // Update mouse start position to current position
            _mouseStartPosition = mousePos;
        }
    }

    private void NodeDragUpdate(Vector2 mousePos)
    {
        if (!dragNode) return;
        if (!_isLmbDown) return;
        // check if mouse has moved more than drag threshold
        var distance = Vector2.Distance(_mouseStartPosition, mousePos);
        if (distance < dragThreshold) return;
            
        // if mouse has moved more than drag threshold, set object position to mouse position
        // keep the y position of the object the same;
        var raycastPosition = GetRaycastPosition(mousePos);
        raycastPosition.y = 1f;
        dragNode.transform.position = raycastPosition;
    }

    private void OnLmbDown (InputAction.CallbackContext callbackContext)
    {
        if (!_inEditMode) return;
        if (!overCanvasCheck.IsOverCanvas) return;
        if (_isRmbDown) return;
        _mouseStartPosition = Mouse.current.position.ReadValue();
        _isLmbDown = true;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cameraRenderRectTransform, Mouse.current.position.ReadValue(), null, out var localPoint);
        var rect = cameraRenderRectTransform.rect;
        var pivot = cameraRenderRectTransform.pivot;
        localPoint.x = ( localPoint.x / rect.width ) + pivot.x;
        localPoint.y = ( localPoint.y / rect.height ) + pivot.x;
        var ray = cam.ViewportPointToRay(localPoint);
        
        // Raycast to check if mouse is over an object
        if (!Physics.Raycast(ray, out var hit)) return;
        
        // Check if object is an ObjectBase
        if (!hit.transform.TryGetComponent(out ObjectBase objectBase)) return;
        
        dragObject = objectBase;
        objectInitialPosition = dragObject.GetPosition();
        OnObjectSelect?.Invoke(objectBase);
    }

    private void OnLmbRelease(InputAction.CallbackContext callbackContext)
    {
        if (!_inEditMode) return;
        // Check if mouse is over canvas
        if (!overCanvasCheck.IsOverCanvas)
        {
            // Check if there is an object to drag
            if (!dragObject) return;
            
            // If there is an object to drag, record the drag action
            var dragAction = new DragAction(objectInitialPosition, dragObject.GetPosition(), dragObject);
            ActionRecorder.Instance.Record(dragAction);
            // Reset drag object
            dragObject = null;
            return;
        }
        
        // Get mouse end position
        _mouseEndPosition = Mouse.current.position.ReadValue();
        
        // Check if mouse has moved more than drag threshold
        var distance = Vector2.Distance(_mouseStartPosition, _mouseEndPosition);
        var isDrag = distance > dragThreshold;
        
        // Reset LMB down
        _isLmbDown = false;
       
        // Raycast to check if mouse is over an object
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cameraRenderRectTransform, Mouse.current.position.ReadValue(), null, out var localPoint);
        var rect = cameraRenderRectTransform.rect;
        var pivot = cameraRenderRectTransform.pivot;
        localPoint.x = ( localPoint.x / rect.width ) + pivot.x;
        localPoint.y = ( localPoint.y / rect.height ) + pivot.x;
        var ray = cam.ViewportPointToRay(localPoint);

        // Check if mouse is over an object
        if (!Physics.Raycast(ray, out var hit)) return;

        if (isDrag) // If is drag, record drag action
        {
            if (dragObject == null) return;
            var dragAction = new DragAction(objectInitialPosition, hit.point, dragObject);
            ActionRecorder.Instance.Record(dragAction);
            dragObject = null;
            return;
        }

        // If click is on an ObjectBase, select the object
        if (hit.transform.TryGetComponent(out ObjectBase objectBase))
        {
            if (ObjectManager.Instance.PrefabToSpawn)
            {
                return;
            }
            OnObjectSelect?.Invoke(objectBase);
            logger.Log("Object selected", this);
            return;
        }

        if (!ObjectManager.Instance.PrefabToSpawn && !ObjectManager.Instance.RoadToolEnabled)
        {
            OnObjectDeselect?.Invoke();
            return;
        }
        OnMapClick?.Invoke(hit.point);
        logger.Log($"Raycast click at {hit.point}", this);
        
        dragObject = null;
    }
    
    private void OnRmbDown(InputAction.CallbackContext callbackContext)
    {
        if (!_inEditMode) return;
        logger.Log("RMB", this);
        if (!overCanvasCheck.IsOverCanvas) return;
        if (_isLmbDown) return;
        _mouseStartPosition = Mouse.current.position.ReadValue();
        _isRmbDown = true;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cameraRenderRectTransform, Mouse.current.position.ReadValue(), null, out var localPoint);
        var rect = cameraRenderRectTransform.rect;
        var pivot = cameraRenderRectTransform.pivot;
        localPoint.x = ( localPoint.x / rect.width ) + pivot.x;
        localPoint.y = ( localPoint.y / rect.height ) + pivot.x;
        var ray = cam.ViewportPointToRay(localPoint);
        if (!Physics.Raycast(ray, out var hit)) return;
        if (!hit.transform.TryGetComponent(out ObjectBase objectBase)) return;
        dragObject = objectBase;
        objectInitialRotation = dragObject.GetRotation();
        OnObjectSelect?.Invoke(objectBase);
    }
    
    private void OnRmbRelease(InputAction.CallbackContext callbackContext)
    {
        if (!_inEditMode) return;
        
        // Reset RMB down
        _isRmbDown = false;
        
        // Check if there is an object to rotate
        if (!dragObject) return;
        
        // Record rotate action
        var rotateAction = new RotateAction(dragObject, objectInitialRotation);
        ActionRecorder.Instance.Record(rotateAction);
        
        // Reset drag object
        dragObject = null;
    }
    
    private void OnPathLmbDown(InputAction.CallbackContext callbackContext)
    {
        _isLmbDown = true;
        if (!_inEditMode) return;
        if (!overCanvasCheck.IsOverCanvas) return;
        _mouseStartPosition = Mouse.current.position.ReadValue();

        var hitTransform = GetRaycastHitTransform(_mouseStartPosition);
        if (!hitTransform) return;
        if (!hitTransform.TryGetComponent(out NodeContainer node)) return;
        dragNode = node;
    }

    private void OnPathLmbRelease(InputAction.CallbackContext callbackContext)
    {
        _isLmbDown = false;
        if (!_inEditMode) return;
        if (!overCanvasCheck.IsOverCanvas) return;
        _mouseEndPosition = Mouse.current.position.ReadValue();
        
        // Check if mouse has moved more than drag threshold
        var distance = Vector2.Distance(_mouseStartPosition, _mouseEndPosition);
        var isDrag = distance > dragThreshold;
        var posEnd = GetRaycastPosition(_mouseEndPosition);
        var posStart = GetRaycastPosition(_mouseStartPosition);
        if (isDrag)
        {
            if (dragNode == null) return;
            var dragAction = new NodeDragAction(posStart, posEnd, dragNode.node);
            ActionRecorder.Instance.Record(dragAction);
        }
        else
        {
            OnPathClick?.Invoke(posEnd);
        }
        dragNode = null;
    }
    
    
    // Get raycast position from mouse position
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

    private Transform GetRaycastHitTransform(Vector2 mousePosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cameraRenderRectTransform, mousePosition, null, out var localPoint);
        var rect = cameraRenderRectTransform.rect;
        var pivot = cameraRenderRectTransform.pivot;
        localPoint.x = ( localPoint.x / rect.width ) + pivot.x;
        localPoint.y = ( localPoint.y / rect.height ) + pivot.x;
        var ray = cam.ViewportPointToRay(localPoint);
        if (Physics.Raycast(ray, out var hit))
        {
            return hit.transform;
        }

        return null;
    }
}

