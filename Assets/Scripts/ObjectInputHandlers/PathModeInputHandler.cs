using Actions;
using Interfaces;
using Managers;
using UnityEngine;

namespace ObjectInputHandlers
{
    class PathModeInputHandler : ObjectInputHandlerBase
    {
        private NodeContainer _nodeContainer;
        private Vector3 _initialPosition;
        
        public PathModeInputHandler(ObjectManager objectManager)
        {
            ObjectManager = objectManager;
        }
        
        public override void HandleMove(IEditorInteractable editorInteractable, Vector3 groundPos)
        {
            if (!ObjectManager.IsOverCanvas)
            {
                HandleOutOfCanvas();

                return;
            }
            
            // Store last ground position
            LastGroundPos = groundPos;
            
            // Check if road container
            var isPathNodeContainer = editorInteractable is NodeContainer;

            // Handle movement if LMB is down
            if (HandleLmbDownMove()) return;
            
            
            // Check if NOT hovering over road point
            if (editorInteractable == null || !isPathNodeContainer)
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

        private void HandleOutOfCanvas()
        {
            IsLmbDown = false;
            IsRmbDown = false;
            if (!ShouldCallDragCommand) return;
            CompleteDrag();
        }

        public override void HandleLmbDown()
        {
            if (!ObjectManager.IsOverCanvas) return;
            IsLmbDown = true;

            // Check if NOT hovering over road point
            if (LastHoveredObject == null)
            {
                // Deselect selected object if any
                SelectedObject?.Deselect();
                SelectedObject = null;
                _nodeContainer = null;

                // Spawn road point
                DesignerManager.Instance.SpawnManager.SpawnPathPoint(LastGroundPos);
                return;
            }

            // If hovering over road point, select it and deselect last selected object
            SelectedObject?.Deselect();
            SelectedObject = LastHoveredObject;
            SelectedObject.Select();
            _nodeContainer = (NodeContainer) SelectedObject;
            _initialPosition = _nodeContainer.transform.position;
        }

        public override void HandleLmbUp()
        {
            if (!ObjectManager.IsOverCanvas) return;
            IsLmbDown = false;
            
            // Check if drag command should be called
            if (!ShouldCallDragCommand) return;
            CompleteDrag();
        }

        private void CompleteDrag()
        {
            // Create action
            var action = new NodeDragAction(_initialPosition, SelectedObject.GetTransform().position, _nodeContainer.Node);
            ActionRecorder.Instance.Record(action);
            ShouldCallDragCommand = false;
        }

        public override void HandleRmbDown()
        {
            if (!ObjectManager.IsOverCanvas) return;
            IsRmbDown = true;
            
            // Check if hovering over road point
            if (LastHoveredObject == null) return;
            
            var pathPointContainer = (NodeContainer) LastHoveredObject;
            
            // Delete road point
            var deleteAction = new PathDeleteAction(pathPointContainer.Node);
            ActionRecorder.Instance.Record(deleteAction);
        }

        public override void HandleRmbUp()
        {
            if (!ObjectManager.IsOverCanvas) return;
            IsRmbDown = false;
        }
        
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
    }
}