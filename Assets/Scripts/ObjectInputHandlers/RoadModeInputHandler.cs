using Actions;
using Interfaces;
using Managers;
using UnityEngine;
using User_Interface;

namespace ObjectInputHandlers
{
    class RoadModeInputHandler : ObjectInputHandlerBase
    {
        private RoadPointContainer _roadPointContainer;
        private Vector3 _initialPosition;
        public RoadModeInputHandler(ObjectManager objectManager)
        {
            ObjectManager = objectManager;
        }
        
        public override void HandleMove(IEditorInteractable editorInteractable, Vector3 groundPos)
        {
            if (!OverViewportCheck.IsOverViewport)
            {
                HandleOutOfCanvas();

                return;
            }
            
            // Store last ground position
            LastGroundPos = groundPos;
            
            // Check if road container
            var isRoadContainer = editorInteractable is RoadPointContainer;

            // Handle movement if LMB is down
            if (HandleLmbDownMove()) return;
            
            
            // Check if NOT hovering over road point
            if (editorInteractable == null || !isRoadContainer)
            {
                LastHoveredObject?.OnPointerExit();
                LastHoveredObject = null;
                return;
            }

            // If hovering over same object, return
            if (LastHoveredObject == editorInteractable) return;
            
            // If hovering over different object, deselect last hovered object and select new one
            LastHoveredObject?.OnPointerExit();
            LastHoveredObject = editorInteractable;
            LastHoveredObject.OnPointerEnter();
        }

        private bool HandleLmbDownMove()
        {
            if (!IsLmbDown) return false;
            bool isDrag = false;
            if (SelectedObject != null)
            {
                isDrag = Vector3.Distance(LastGroundPos, _initialPosition) > ObjectManager.DragThreshold;
            }

            if (!isDrag) return true;
            // Check if object is selected
            SelectedObject?.OnDrag(LastGroundPos);
            ShouldCallDragCommand = true;
            return true;
        }


        public override void HandleLmbDown()
        {
            if (!OverViewportCheck.IsOverViewport) return;
            IsLmbDown = true;

            // Check if NOT hovering over road point
            if (LastHoveredObject == null)
            {
                // Deselect selected object if any
                SelectedObject?.Deselect();
                SelectedObject = null;
                _roadPointContainer = null;

                // Spawn road point
                DesignerManager.Instance.SpawnManager.AddRoadPoint(LastGroundPos);
                return;
            }

            // If hovering over road point, select it and deselect last selected object
            SelectedObject?.Deselect();
            SelectedObject = LastHoveredObject;
            SelectedObject.Select();
            _roadPointContainer = (RoadPointContainer) SelectedObject;
            _initialPosition = _roadPointContainer.transform.position;
        }

        public override void HandleLmbUp()
        {
            if (!OverViewportCheck.IsOverViewport) return;
            IsLmbDown = false;
            
            // Check if drag command should be called
            if (!ShouldCallDragCommand) return;
            CompleteDrag();
        }

        public override void HandleRmbDown()
        {
            if (!OverViewportCheck.IsOverViewport) return;
            IsRmbDown = true;
            
            // Check if hovering over road point
            if (LastHoveredObject == null) return;
            
            var roadPointContainer = (RoadPointContainer) LastHoveredObject;
            
            // Delete road point
            var deleteAction = new RoadDeleteAction(roadPointContainer.RoadPoint);
            ActionRecorder.Instance.Record(deleteAction);
            
        }

        public override void HandleRmbUp()
        {
            if (!OverViewportCheck.IsOverViewport) return;
            IsRmbDown = false;
        }
        
        
        // Cleans up selected or hovered road points on exit
        public override void CleanUp(EditMode editMode)
        {
            if (SelectedObject != null)
            {
                SelectedObject?.Deselect();
                SelectedObject = null;
            }

            if (LastHoveredObject == null) return;
            LastHoveredObject?.OnPointerExit();
            LastHoveredObject = null;
        }

        /// <summary>
        /// Create drag action and record it
        /// </summary>
        private void CompleteDrag()
        {
            // Create action
            var action = new RoadDragAction(_initialPosition, SelectedObject.GetTransform().position, _roadPointContainer.RoadPoint);
            ActionRecorder.Instance.Record(action);
            ShouldCallDragCommand = false;
        }
        
        /// <summary>
        /// Handles mouse going out of canvas
        /// </summary>
        private void HandleOutOfCanvas()
        {
            IsLmbDown = false;
            IsRmbDown = false;
            if (!ShouldCallDragCommand) return;
            CompleteDrag();
        }
        
    }
}