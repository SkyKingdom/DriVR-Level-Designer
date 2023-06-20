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
            _inputManager = DesignerManager.Instance.InputManager;
            inputHandlerDelegate = new ObjectModeInputHandler(this);
        }

        private void OnEnable()
        {
            // Subscribe to events
            DesignerManager.Instance.OnEditTypeChange += OnEditTypeChanged;
            _inputManager.OnLmbDown += OnLMBDown;
            _inputManager.OnLmbUp += OnLMBUp;
            _inputManager.OnRmbDown += OnRMBDown;
            _inputManager.OnRmbUp += OnRMBUp;
            _inputManager.OnMouseMove += OnMouseMove;
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

        private void OnEditTypeChanged(EditMode oldValue, EditMode value)
        {

            inputHandlerDelegate?.CleanUp(value);
            switch (value)
            {
                case EditMode.Object:
                    inputHandlerDelegate = new ObjectModeInputHandler(this);
                    var lastSelected = DesignerManager.Instance.SelectionManager.LastSelectedObject;
                    if (lastSelected != null)
                        inputHandlerDelegate.SetSelectedObject(lastSelected);
                    break;
                case EditMode.Path:
                    inputHandlerDelegate = new PathModeInputHandler(this);
                    break;
                case EditMode.Road:
                    inputHandlerDelegate = new RoadModeInputHandler(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        private void OnMouseMove(IEditorInteractable editorObj, Vector3 groundPos) => inputHandlerDelegate.HandleMove(editorObj, groundPos);

        private void OnRMBUp() => inputHandlerDelegate.HandleRmbUp();

        private void OnRMBDown() => inputHandlerDelegate.HandleRmbDown();

        private void OnLMBUp()
        {
            inputHandlerDelegate.HandleLmbUp();
        }

        private void OnLMBDown()
        {
            inputHandlerDelegate.HandleLmbDown();
        }
    }
}