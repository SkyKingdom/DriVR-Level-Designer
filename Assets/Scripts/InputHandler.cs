using System;
using Interfaces;
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

    
    private IEditorInteractable _currentHoveredObject;
    private IEditorInteractable _currentSelectedObject;
    
    // States
    private bool InEditMode => LevelGeneratorManager.Instance.Mode == Mode.Edit;
    
    // Start and end positions of the mouse drag
    private Vector2 _mouseStartPosition;
    private Vector2 _mouseEndPosition;
    
    // Events
    public event Action OnSelect;
    public event Action OnDeselect;
    public event Action<Vector3> OnSpawnClick;

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
        ObjectManager.Instance.ObjectSpawned += OnObjectSpawned;
        _move.performed += OnMove;

        _lmb.started += OnLMBDown;
        _lmb.canceled += OnLMBUp;
    }
    
    private void OnEnable()
    {
        _lmb.Enable();
        _rmb.Enable();
        _move.Enable();
    }
    
    private void OnDisable()
    {
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

        if (_rmbDown)
        {
            
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
            return;
        }

        // If hovered object is interactable, hover over it
        if (!hit.transform.TryGetComponent(out IEditorInteractable interactable)) return;
        interactable.OnPointerEnter();
        _currentHoveredObject = interactable;
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
    
    #endregion

    #region Private Methods
    
    private void HandleClick()
    {
        var ray = GetRayFromMousePosition(Mouse.current.position.ReadValue());

        // Check if prefab to spawn is selected
        if (ObjectManager.Instance.PrefabToSpawn)
        {
            // Check if raycast hits ground
            if (!Physics.Raycast(ray, out var spawnHit)) return;
            if (!groundMask.Contains(spawnHit.transform.gameObject.layer)) return;
            
            OnSpawnClick?.Invoke(spawnHit.point);
            
            return;
        }

        if (_currentHoveredObject == null)
        {
            DeselectObject();
            OnDeselect?.Invoke();
            return;
        }

        _currentSelectedObject?.Deselect();
        _currentHoveredObject?.Select();
        _currentSelectedObject = _currentHoveredObject;
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
        // TODO: Handle object deselection
        _currentSelectedObject?.Deselect();
        _currentSelectedObject = null;
    }

    #endregion

}