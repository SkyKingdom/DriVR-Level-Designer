using System;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [Header("Ray Casting")]
        [SerializeField] private RectTransform viewportRenderTexture; // Render texture viewport
        [SerializeField] private Camera sceneCamera; // Scene camera to cast ray from
        [SerializeField] private LayerMask objectsMask; // Layer mask for objects
        [SerializeField] private LayerMask groundMask; // Layer mask for ground
    
        [Header("Input")] 
        [SerializeField] private InputActionAsset inputActionAsset;

        private InputAction _lmb;
        private InputAction _rmb;
        private InputAction _move;

        [Header("Settings")] 
        [SerializeField] private float dragThreshold = 1f; // Minimum movement to trigger drag

        [SerializeField] private float rotationSpeed = 3f; // Rotation speed of objects

    
        #region Events

        public event Action<IEditorInteractable, Vector3> OnMouseMove;
        public event Action OnLmbDown;
        public event Action OnLmbUp;
        public event Action OnRmbDown;
        public event Action OnRmbUp;

        #endregion

        #region Unity Methods

        protected void Awake()
        {
            // Get input actions
            _lmb = inputActionAsset.FindActionMap("Default").FindAction("LMB");
            _rmb = inputActionAsset.FindActionMap("Default").FindAction("RMB");
            _move = inputActionAsset.FindActionMap("Default").FindAction("Move");
        }

        private void OnEnable()
        {
            // Enable input actions
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
            // Disable input actions
            _move.performed -= OnMove;

            _lmb.started -= OnLMBDown;
            _lmb.canceled -= OnLMBUp;
        
            _rmb.started -= OnRMBDown;
            _rmb.canceled -= OnRMBUp;
        
            _lmb.Disable();
            _rmb.Disable();
            _move.Disable();
        }
    
        #endregion

        #region Input Methods

        private void OnMove(InputAction.CallbackContext obj)
        {
            var ray = GetRayFromMousePosition(obj.ReadValue<Vector2>());
            IEditorInteractable editorObj = null;
            Vector3 pos = Vector3.zero;
            // Remove hover from previous object if raycast doesn't hit anything
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, objectsMask))
            { 
                editorObj = hit.transform.GetComponent<IEditorInteractable>();
            }
            pos = GetGroundHitPosition(ray);
            OnMouseMove?.Invoke(editorObj, pos);
        }

        private void OnLMBDown(InputAction.CallbackContext obj) => OnLmbDown?.Invoke();

        private void OnLMBUp(InputAction.CallbackContext obj) => OnLmbUp?.Invoke();

        private void OnRMBDown(InputAction.CallbackContext obj) => OnRmbDown?.Invoke();

        private void OnRMBUp(InputAction.CallbackContext obj) => OnRmbUp?.Invoke();

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

        #endregion
    }
}