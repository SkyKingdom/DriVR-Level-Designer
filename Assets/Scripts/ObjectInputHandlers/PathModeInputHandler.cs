using Actions;
using Interfaces;
using Managers;
using UnityEngine;
using User_Interface;

namespace ObjectInputHandlers
{
    class PathModeInputHandler : ObjectInputHandlerBase
    {
        public PathModeInputHandler(ObjectManager objectManager)
        {
            ObjectManager = objectManager;
        }
        
        public override void HandleMove(IEditorInteractable editorInteractable, Vector3 groundPos)
        {
            if (!OverViewportCheck.IsOverViewport) return;
            
            // Check if road container
            var isPathNodeContainer = editorInteractable is PathPointContainer;

            // Store last ground position
            LastGroundPos = groundPos;
            
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

        public override void HandleLmbDown()
        {
            if (!OverViewportCheck.IsOverViewport) return;
            IsLmbDown = true;

            // Check if NOT hovering over road point
            if (LastHoveredObject == null)
            {
                // Deselect selected object if any
                ClearSelection();

                // Spawn road point
                DesignerManager.Instance.SpawnManager.SpawnPathPoint(LastGroundPos);
                return;
            }

            if (LastHoveredObject == SelectedObject) return;
            
            // If hovering over road point, select it and deselect last selected object
            ClearSelection();
            SelectedObject = LastHoveredObject;
            SelectedObject.Select();
            SelectedObject.OnObjectDeleted += ClearObject;
        }
        
        private void ClearSelection()
        {
            if (SelectedObject == null) return;
            // Unsubscribe from event
            SelectedObject.OnObjectDeleted -= ClearObject;
            SelectedObject.Deselect();
            SelectedObject = null;
        }

        private void ClearObject()
        {
            SelectedObject.OnObjectDeleted -= ClearObject;
            SelectedObject = null;
        }

        public override void HandleLmbUp()
        {
            IsLmbDown = false;
        }

        public override void HandleRmbDown()
        {
            if (!OverViewportCheck.IsOverViewport) return;
            IsRmbDown = true;
            
            // Check if hovering over road point
            if (LastHoveredObject == null) return;
            
            var pathPointContainer = (PathPointContainer) LastHoveredObject;
            LastHoveredObject?.OnPointerExit();
            LastHoveredObject = null;
            // Delete road point
            var deleteAction = new PathDeleteAction(pathPointContainer.PathPoint);
            ActionRecorder.Instance.Record(deleteAction);
        }

        public override void HandleRmbUp()
        {
            IsRmbDown = false;
        }
        
        public override void CleanUp(EditMode editMode)
        {
            IsLmbDown = false;
            IsRmbDown = false;
            LastHoveredObject?.OnPointerExit();
            LastHoveredObject = null;
            ClearSelection();
        }
    }
}