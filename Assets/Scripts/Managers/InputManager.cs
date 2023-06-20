using System;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        [Header("Ray Casting")]
        [SerializeField] private Camera sceneCamera; // Scene camera to cast ray from
        [SerializeField] private LayerMask objectsMask; // Layer mask for objects
        [SerializeField] private LayerMask groundMask; // Layer mask for ground
        [SerializeField] private LayerMask handlesMask;
    
        [Header("Input")] 
        [SerializeField] private InputActionAsset inputActionAsset;

        private InputAction _lmb;
        private InputAction _rmb;
        private InputAction _move;
        private InputAction _shift;
    
        #region Events

        public event Action<IEditorInteractable, Vector3> OnMouseMove;
        public event Action OnLmbDown;
        public event Action OnLmbUp;
        public event Action OnRmbDown;
        public event Action OnRmbUp;
        
        public event Action OnShiftDown;
        
        public event Action OnShiftUp;

        #endregion

        #region Unity Methods

        protected void Awake()
        {
            // Get input actions
            _lmb = inputActionAsset.FindActionMap("Default").FindAction("LMB");
            _rmb = inputActionAsset.FindActionMap("Default").FindAction("RMB");
            _move = inputActionAsset.FindActionMap("Default").FindAction("Move");
            _shift = inputActionAsset.FindActionMap("Default").FindAction("Shift");
        }

        private void OnEnable()
        {
            // Enable input actions
            _lmb.Enable();
            _rmb.Enable();
            _move.Enable();
            _shift.Enable();
        
            _move.performed += OnMove;

            _lmb.started += OnLMBDown;
            _lmb.canceled += OnLMBUp;
        
            _rmb.started += OnRMBDown;
            _rmb.canceled += OnRMBUp;
            
            _shift.started += ShiftDown;
            _shift.canceled += ShiftUp;
        }

        private void OnDisable()
        {
            // Disable input actions
            _move.performed -= OnMove;

            _lmb.started -= OnLMBDown;
            _lmb.canceled -= OnLMBUp;
        
            _rmb.started -= OnRMBDown;
            _rmb.canceled -= OnRMBUp;
            
            _shift.started -= ShiftDown;
            _shift.canceled -= ShiftUp;
        
            _lmb.Disable();
            _rmb.Disable();
            _move.Disable();
            _shift.Disable();
        }
    
        #endregion

        #region Input Methods

        // Handles mouse move event
        private void OnMove(InputAction.CallbackContext obj)
        {
            var ray = GetRayFromMousePosition(); // Get ray from mouse position
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

        private void OnLMBDown(InputAction.CallbackContext obj)
        {
            if (IsOverHandle()) return;
            OnLmbDown?.Invoke();
        }

        private void OnLMBUp(InputAction.CallbackContext obj)
        {
            if (IsOverHandle()) return;
            OnLmbUp?.Invoke();
        }

        private void OnRMBDown(InputAction.CallbackContext obj)
        {
            if (IsOverHandle()) return;
            OnRmbDown?.Invoke();
        }

        private void OnRMBUp(InputAction.CallbackContext obj)
        {
            if (IsOverHandle()) return;
            OnRmbUp?.Invoke();
        }
        
        private void ShiftDown(InputAction.CallbackContext obj)
        {
            OnShiftDown?.Invoke();
        }
        
        private void ShiftUp(InputAction.CallbackContext obj)
        {
            OnShiftUp?.Invoke();
        }

        #endregion
        
        #region Helper Methods

        // Get ray from mouse position
        private Ray GetRayFromMousePosition()
        {
            return sceneCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        }
    
        // Get ground hit position
        private Vector3 GetGroundHitPosition(Ray ray)
        {
            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, groundMask)) return Vector3.zero;
            var hitPos = hit.point;
            hitPos.y = 0;
            return hitPos;
        }
        
        // Check if mouse is over transform handle
        private bool IsOverHandle()
        {
            var ray = GetRayFromMousePosition();
            return Physics.Raycast(ray, out var hit, Mathf.Infinity, handlesMask);
        }

        #endregion
    }
}