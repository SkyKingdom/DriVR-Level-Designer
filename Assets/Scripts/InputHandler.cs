using System;
using Interfaces;
using Objects;
using UnityEngine;
using UnityEngine.InputSystem;
using User_Interface;
using Utilities;

public class InputHandler : StaticInstance<InputHandler>
{
    [Header("Ray Casting")]
    [SerializeField] private RectTransform viewportRenderTexture;
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private OverViewportCheck overViewportCheck;
    [SerializeField] private LayerMask objectsMask;
    [SerializeField] private LayerMask groundMask;


    [Header("Input")] 
    [SerializeField] private InputActionAsset inputActionAsset;
    
    private InputAction _lmb;
    private InputAction _rmb;
    private InputAction _move;

    private bool _lmbDown;
    private bool _rmbDown;
    
    [Header("Settings")] 
    [SerializeField] private float dragThreshold = 1f;

    [SerializeField] private float rotationSpeed = 3f;
    
    
    private IEditorInteractable _currentHoveredObject;
    private IEditorInteractable _currentSelectedObject;

    [SerializeField] private ObjectBase _selectedObjectBase;
    [SerializeField] private ObjectBase _hoveredObjectBase;

    // States
    private bool InEditMode => LevelGeneratorManager.Instance.Mode == Mode.Edit;
    
    // Start and end positions of the mouse drag
    private Vector2 _mouseStartPosition;
    private Vector2 _mouseEndPosition;
    private Vector2 _lastMousePosition;
    
    // Events
    public event Action<ObjectBase> OnSelect;
    public event Action OnDeselect;
    public event Action<Vector3> OnSpawn;


    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();
        
        _lmb = inputActionAsset.FindActionMap("Default").FindAction("LMB");
        _rmb = inputActionAsset.FindActionMap("Default").FindAction("RMB");
        _move = inputActionAsset.FindActionMap("Default").FindAction("Move");
    }

    private void Start()
    {
        LevelGeneratorManager.Instance.OnModeChange += OnModeChanged;
        SpawnManager.Instance.ObjectSpawned += OnObjectSpawned;
        SpawnManager.Instance.EditTypeChanged += OnEditTypeChanged;
    }

    private void OnEnable()
    {
        _lmb.Enable();
        _rmb.Enable();
        _move.Enable();
        
        _move.performed += OnMove;

        _lmb.started += OnLMBDown;
        _lmb.canceled += OnLMBUp;
        
        _rmb.started += OnRMBDown;
        _rmb.canceled += OnRMBUp;
    }
    
    private void OnDisable()
    {
        _move.performed -= OnMove;

        _lmb.started -= OnLMBDown;
        _lmb.canceled -= OnLMBUp;
        
        _rmb.started -= OnRMBDown;
        _rmb.canceled -= OnRMBUp;
        
        _lmb.Disable();
        _rmb.Disable();
        _move.Disable();
    }

    private void Update()
    {
        if (_currentSelectedObject == null) return;
        var mousePos = Mouse.current.position.ReadValue();
        if (_lmbDown && Vector2.Distance(_mouseStartPosition, mousePos) > dragThreshold)
        {
            _currentSelectedObject.OnDrag(GetGroundHitPosition(GetRayFromMousePosition(mousePos)));
            return;
        }

        if (_rmbDown && Vector2.Distance(_mouseStartPosition, mousePos) > dragThreshold)
        {
            float mouseTravel = CalculateMouseTravel(mousePos);
            _currentSelectedObject.OnRotate(mouseTravel);
        }
    }
    
    #endregion

    #region Event Handlers

    private void OnModeChanged()
    {
        if (InEditMode) return;
        
        DeselectObject();
    }
    
    private void OnObjectSpawned(IEditorInteractable obj)
    {
        _currentSelectedObject = obj;
        _currentSelectedObject.Select();
    }
    
    
    private void OnEditTypeChanged(EditType exitMode, EditType enterMode)
    {
        if (exitMode == EditType.Path && enterMode == EditType.Object)
        {
            _currentSelectedObject?.Deselect();
            _currentSelectedObject = _selectedObjectBase;
        }
    }

    #endregion
    
    #region Input Methods

    private void OnMove(InputAction.CallbackContext obj)
    {
        if (!InEditMode) return;
        if (!overViewportCheck.IsOverViewport) return;
        if (_rmbDown || _lmbDown) return;
        
        var ray = GetRayFromMousePosition(obj.ReadValue<Vector2>());
        
        // Remove hover from previous object if raycast doesn't hit anything
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, objectsMask))
        { 
            _currentHoveredObject?.OnPointerExit();
            _currentHoveredObject = null;
            _hoveredObjectBase = null;
            return;
        }

        // If hovered object is interactable, hover over it
        if (!hit.transform.TryGetComponent(out IEditorInteractable interactable)) return;
        interactable.OnPointerEnter();
        _currentHoveredObject = interactable;
        _hoveredObjectBase = hit.transform.GetComponent<ObjectBase>();
    }
    
    private void OnLMBDown(InputAction.CallbackContext obj)
    {
        if (!InEditMode) return;
        if (!overViewportCheck.IsOverViewport) return;
        if (_rmbDown) return;
        
        _lmbDown = true;
        _mouseStartPosition = Mouse.current.position.ReadValue();
        
        HandleClick();
    }
    
    private void OnLMBUp(InputAction.CallbackContext obj)
    {
        if (!InEditMode) return;
        if (!overViewportCheck.IsOverViewport) return;
        if (_rmbDown) return;
        
        _lmbDown = false;
        _mouseEndPosition = Mouse.current.position.ReadValue();
        
        bool isDrag = Vector2.Distance(_mouseStartPosition, _mouseEndPosition) > dragThreshold;

        if (isDrag)
        {
            _currentSelectedObject?.OnDragRelease();
        }
    }
    
    private void OnRMBDown(InputAction.CallbackContext obj)
    {
        if (!InEditMode) return;
        if (!overViewportCheck.IsOverViewport) return;
        if (_lmbDown) return;
        
        _rmbDown = true;
        _mouseStartPosition = Mouse.current.position.ReadValue();
    }
    
    private void OnRMBUp(InputAction.CallbackContext obj)
    {
        if (!InEditMode) return;
        if (!overViewportCheck.IsOverViewport) return;
        if (_lmbDown) return;
        
        _rmbDown = false;
        _mouseEndPosition = Mouse.current.position.ReadValue();
        
        bool isDrag = Vector2.Distance(_mouseStartPosition, _mouseEndPosition) > dragThreshold;

        if (isDrag)
        {
            _currentSelectedObject?.OnRotateRelease();
        }
    }
    
    #endregion

    #region Private Methods
    
    private void HandleClick()
    {
        if (_currentHoveredObject != null)
        {
            SelectHoveredObject();
            return;
        }
        
        var ray = GetRayFromMousePosition(Mouse.current.position.ReadValue());

        // Check if raycast hits ground
        if (!Physics.Raycast(ray, out var spawnHit)) return;

        if (SpawnManager.Instance.EditType == EditType.Object && !SpawnManager.Instance.PrefabToSpawn)
        {
            DeselectObject();
                return;
        }
        
        switch (SpawnManager.Instance.EditType)
        {
            case EditType.Object:
                OnSpawn?.Invoke(spawnHit.point);
                return;
            case EditType.Path:
                OnSpawn?.Invoke(spawnHit.point);
                return;
            case EditType.Road:
                OnSpawn?.Invoke(spawnHit.point);
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private float CalculateMouseTravel(Vector2 mousePos)
    {
        var distance = _lastMousePosition.x - mousePos.x;
        if (Mathf.Abs(distance) > dragThreshold)
        {
            // Change rotation direction based on distance direction
            distance = distance switch
            {
                > 0 => rotationSpeed,
                < 0 => -rotationSpeed,
                _ => distance
            };
        }
        // Update mouse start position to current position
        _lastMousePosition = mousePos;
        return distance;
    }
    
    private void SelectHoveredObject()
    {
        switch (SpawnManager.Instance.EditType)
        {
            case EditType.Object:
                _currentSelectedObject?.Deselect();
                _currentSelectedObject = _currentHoveredObject;
                _selectedObjectBase = _hoveredObjectBase;
                _currentSelectedObject?.Select();
                OnSelect?.Invoke(_selectedObjectBase);
                break;
            case EditType.Path:
                _currentSelectedObject = _currentHoveredObject;
                break;
            case EditType.Road:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion

    #region Helper Methods

    private Ray GetRayFromMousePosition(Vector2 mousePosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(viewportRenderTexture, mousePosition, null,
            out var localPoint);
        var rect = viewportRenderTexture.rect;
        var pivot = viewportRenderTexture.pivot;
        localPoint.x = (localPoint.x / rect.width) + pivot.x;
        localPoint.y = (localPoint.y / rect.height) + pivot.x;
        return sceneCamera.ViewportPointToRay(localPoint);
    }
    
    private Vector3 GetGroundHitPosition(Ray ray)
    {
        if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, groundMask)) return Vector3.zero;
        var hitPos = hit.point;
        hitPos.y = 0;
        return hitPos;
    }

    private void DeselectObject()
    {
        _currentSelectedObject?.Deselect();
        _currentSelectedObject = null;
        _selectedObjectBase = null;
        OnDeselect?.Invoke();
    }

    #endregion

    public void DropObject(ObjectBase obj)
    {
        if (_selectedObjectBase != obj) return;
        DeselectObject();
    }

    public void SelectObject(ObjectBase obj)
    {
        _currentSelectedObject?.Deselect();
        _currentSelectedObject = obj;
        _selectedObjectBase = obj;
        _currentSelectedObject?.Select();
        OnSelect?.Invoke(_selectedObjectBase);
    }
}