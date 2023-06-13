using Actions;
using Interfaces;
using Managers;
using Objects;
using UnityEngine;

namespace ObjectInputHandlers
{
    class ObjectModeInputHandler : ObjectInputHandlerBase
    {
        private ObjectBase _objectBase;
        private Vector3 _initialPosition;
        private Vector3 _initialRotation;
        private Vector3 _rotationPosition;
        
        public ObjectModeInputHandler(ObjectManager objectManager)
        {
            ObjectManager = objectManager;
        }
        
        // Handles mouse movement
        public override void HandleMove(IEditorInteractable editorInteractable, Vector3 groundPos)
        {
            if (!ObjectManager.IsOverCanvas)
            {
                HandleOutOfCanvas();
                
                return;
            }
            
            var isObject = editorInteractable is ObjectBase;
            
            LastGroundPos = groundPos;
            
            // Handle movement if LMB is down
            if (HandleLmbDownMove()) return;
            
            if (HandleRmbDownMove()) return;
            
            // Check if NOT hovering over object
            if (editorInteractable == null || !isObject)
            {
                LastHoveredObject?.OnPointerExit();
                LastHoveredObject = null;
                return;
            }
            
            // If hovering over same object, return
            if (LastHoveredObject == editorInteractable) return;
            
            // If hovering over new object, exit last object and set new object
            LastHoveredObject?.OnPointerExit();
            LastHoveredObject = editorInteractable;
            LastHoveredObject?.OnPointerEnter();
        }

        // Returns true if LMB is down and object is being rotated
        private bool HandleRmbDownMove()
        {
            if (!IsRmbDown) return false;
            bool isDrag = false;
            float distance = _rotationPosition.x - LastGroundPos.x;
            if (SelectedObject != null)
            {
                isDrag = Mathf.Abs(distance) > ObjectManager.DragThreshold/100;
            }
            
            if (!isDrag) return true;

            distance = distance switch
            {
                > 0 => ObjectManager.RotationSpeed,
                < 0 => -ObjectManager.RotationSpeed,
                _ => distance
            };
            
            SelectedObject?.OnRotate(distance);
            _rotationPosition = LastGroundPos;
            ShouldCallDragCommand = true;
            return true;
        }

        // Returns true if LMB is down and object is being dragged
        private bool HandleLmbDownMove()
        {
            if (!IsLmbDown) return false;
            bool isDrag = false;
            if (SelectedObject != null)
            {
                isDrag = Vector3.Distance(LastGroundPos, _initialPosition) > ObjectManager.DragThreshold;
            }

            if (!isDrag) return true;
            SelectedObject?.OnDrag(LastGroundPos);
            ShouldCallDragCommand = true;
            return true;

        }
        
        public override void HandleLmbDown()
        {
            if (!ObjectManager.IsOverCanvas) return;
            IsLmbDown = true;
            
            // Check if hovering over object
            if (LastHoveredObject == null)
            {
                // If not hovering over object, deselect object
                SelectedObject?.Deselect();
                SelectedObject = null;
                _objectBase = null;
                DesignerManager.Instance.SelectionManager.DeselectObject();
                // Spawn new object
                DesignerManager.Instance.SpawnManager.SpawnObject(LastGroundPos);
                return;
            }
            
            // If hovering over object, select object
            SelectedObject?.Deselect();
            SelectedObject = LastHoveredObject;
            SelectedObject.Select();
            _objectBase = (ObjectBase)SelectedObject;
            _initialPosition = _objectBase.GetPosition();
            _rotationPosition = _initialPosition;
            _initialRotation = _objectBase.GetRotation();
        }

        public override void HandleLmbUp()
        {
            if (!ObjectManager.IsOverCanvas) return;
            IsLmbDown = false;
            
            if (!ShouldCallDragCommand) return;
            CompleteDrag();
        }

        // Records drag action
        private void CompleteDrag()
        {
            var action = new DragAction(_initialPosition, _objectBase.GetPosition(), _objectBase);
            ActionRecorder.Instance.Record(action);
            ShouldCallDragCommand = false;
        }

        public override void HandleRmbDown()
        {
            if (!ObjectManager.IsOverCanvas) return;
            
            IsRmbDown = true;
            
            // Check if hovering over object
            if (LastHoveredObject == null)
            {
                return;
            }
            
            // If hovering over object, select object
            SelectedObject?.Deselect();
            SelectedObject = LastHoveredObject;
            SelectedObject.Select();
            _objectBase = (ObjectBase)SelectedObject;
            _initialPosition = _objectBase.GetPosition();
            _rotationPosition = _initialPosition;
            _initialRotation = _objectBase.GetRotation();
        }

        public override void HandleRmbUp()
        {
            if (!ObjectManager.IsOverCanvas) return;
            
            IsRmbDown = false;
            
            if (!ShouldCallDragCommand) return;
            CompleteRotation();
        }

        // Records rotation action
        private void CompleteRotation()
        {
            var action = new RotateAction(_initialRotation, _objectBase.GetRotation(), _objectBase);
            ActionRecorder.Instance.Record(action);
            ShouldCallDragCommand = false;
        }

        
        // Cleans up on mode change
        public override void CleanUp(EditMode editMode)
        {
            IsLmbDown = false;
            IsRmbDown = false;
            ShouldCallDragCommand = false;
            LastHoveredObject?.OnPointerExit();
            LastHoveredObject = null;
            if (editMode == EditMode.Road)
            {
                SelectedObject?.Deselect();
                DesignerManager.Instance.SelectionManager.DeselectObject();
            }

            SelectedObject = null;
            _objectBase = null;
        }
        
        /// <summary>
        /// Handles mouse going out of canvas
        /// </summary>
        private void HandleOutOfCanvas()
        {
            
            if (ShouldCallDragCommand)
            {
                if(IsRmbDown) CompleteRotation();
                else CompleteDrag();
            }
            
            IsLmbDown = false;
            IsRmbDown = false;
        }
    }
}