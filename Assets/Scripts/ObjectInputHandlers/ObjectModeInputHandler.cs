using Actions;
using Interfaces;
using Managers;
using Objects;
using UnityEngine;
using User_Interface;

namespace ObjectInputHandlers
{
    class ObjectModeInputHandler : ObjectInputHandlerBase
    {
        public ObjectModeInputHandler(ObjectManager objectManager)
        {
            ObjectManager = objectManager;
        }
        
        // Handles mouse movement
        public override void HandleMove(IEditorInteractable editorInteractable, Vector3 groundPos)
        {
            if (!OverViewportCheck.IsOverViewport) return;

            // Check if object
            var isObject = editorInteractable is ObjectBase;
            
            // Store last ground position
            LastGroundPos = groundPos;
            
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

        
        public override void HandleLmbDown()
        {
            if (!OverViewportCheck.IsOverViewport) return;
            IsLmbDown = true;
            
            // Check if hovering over object
            if (LastHoveredObject == null)
            {
                // If not hovering over object, deselect object
                ClearSelection();
                // Spawn new object
                DesignerManager.Instance.SpawnManager.SpawnObject(LastGroundPos);
                return;
            }
            
            // If same object, return
            if (LastHoveredObject == SelectedObject) return;
            
            // If hovering over object, select object
            ClearSelection();
            SelectedObject = LastHoveredObject;
            SelectedObject.Select();
            SelectedObject.OnObjectDeleted += ClearObject;
        }

        private void ClearSelection(bool shouldDeselect = true)
        {
            if (SelectedObject == null) return;
            // Unsubscribe from event
            SelectedObject.OnObjectDeleted -= ClearObject;
            
            if (shouldDeselect)
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
            
            // Check if hovering over object
            if (LastHoveredObject == null) return;

            // If same object, return
            if (LastHoveredObject == SelectedObject) return;
            
            // If hovering over object, select object
            ClearSelection();
            SelectedObject = LastHoveredObject;
            SelectedObject.Select();
            SelectedObject.OnObjectDeleted += ClearObject;
        }

        public override void HandleRmbUp()
        {
            IsRmbDown = false;
        }

        
        // Cleans up on mode change
        public override void CleanUp(EditMode editMode)
        {
            IsLmbDown = false;
            IsRmbDown = false;
            LastHoveredObject?.OnPointerExit();
            LastHoveredObject = null;
            ClearSelection(editMode == EditMode.Road || editMode == EditMode.None);
        }

        public override void SetSelectedObject(IEditorInteractable editorInteractable)
        {
            if (SelectedObject != null)
            {
                SelectedObject.Deselect();
            }
            SelectedObject = editorInteractable;
            SelectedObject.Select();
        }
    }
}