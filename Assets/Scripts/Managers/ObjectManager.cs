using System;
using Interfaces;
using ObjectInputHandlers;
using UnityEngine;
using User_Interface;

namespace Managers
{
    public class ObjectManager : MonoBehaviour
    {
        private InputManager _inputManager;
        private ObjectInputHandlerBase inputHandlerDelegate;

        #region Settings

        [field: SerializeField] public float DragThreshold { get; private set; } = 1f; // Minimum movement to trigger drag
        [field: SerializeField] public float RotationSpeed { get; private set; } = 5f; // Object rotation speed

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // Get dependencies
            _inputManager = DesignerManager.Instance.InputManager;
            
            // Set default input handler
            inputHandlerDelegate = new ObjectModeInputHandler(this);
        }

        private void OnEnable()
        {
            // Subscribe to events
            DesignerManager.Instance.OnEditTypeChange += OnEditTypeChanged;
            DesignerManager.Instance.OnModeChange += OnModeChange;
            _inputManager.OnLmbDown += OnLMBDown;
            _inputManager.OnLmbUp += OnLMBUp;
            _inputManager.OnRmbDown += OnRMBDown;
            _inputManager.OnRmbUp += OnRMBUp;
            _inputManager.OnMouseMove += OnMouseMove;
        }

        private void OnModeChange(Mode oldValue, Mode value)
        {
            if (value != Mode.Edit)
            {
                // Clean up the input handler delegate when leaving edit mode
                inputHandlerDelegate.CleanUp(EditMode.None);
                inputHandlerDelegate = new NoEditInputHandler();
                return;
            }
            inputHandlerDelegate = new ObjectModeInputHandler(this);
        }

        private void OnDisable()
        {
            // Unsubscribe from events
            DesignerManager.Instance.OnEditTypeChange -= OnEditTypeChanged;
            _inputManager.OnLmbDown -= OnLMBDown;
            _inputManager.OnLmbUp -= OnLMBUp;
            _inputManager.OnRmbDown -= OnRMBDown;
            _inputManager.OnRmbUp -= OnRMBUp;
            _inputManager.OnMouseMove -= OnMouseMove;
        }

        #endregion

        // Handles the edit type change
        private void OnEditTypeChanged(EditMode oldValue, EditMode value)
        {
            inputHandlerDelegate?.CleanUp(value);
            switch (value)
            {
                case EditMode.Object:
                    inputHandlerDelegate = new ObjectModeInputHandler(this); // Changes the input handler delegate to the object mode input handler
                    var lastSelected = DesignerManager.Instance.SelectionManager.LastSelectedObject;
                    if (lastSelected != null)
                        inputHandlerDelegate.SetSelectedObject(lastSelected);
                    break;
                case EditMode.Path:
                    inputHandlerDelegate = new PathModeInputHandler(this); // Changes the input handler delegate to the path mode input handler
                    break;
                case EditMode.Road:
                    inputHandlerDelegate = new RoadModeInputHandler(this); // Changes the input handler delegate to the road mode input handler
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        // Handles mouse movement using the input handler delegate
        private void OnMouseMove(IEditorInteractable editorObj, Vector3 groundPos) => inputHandlerDelegate.HandleMove(editorObj, groundPos);

        // Handles right mouse button up and down using the input handler delegate
        private void OnRMBUp() => inputHandlerDelegate.HandleRmbUp();
        private void OnRMBDown() => inputHandlerDelegate.HandleRmbDown();

        // Handles left mouse button up and down using the input handler delegate
        private void OnLMBUp() => inputHandlerDelegate.HandleLmbUp();
        private void OnLMBDown() => inputHandlerDelegate.HandleLmbDown();
    }
}