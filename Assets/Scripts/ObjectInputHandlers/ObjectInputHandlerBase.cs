using Interfaces;
using Managers;
using Vector3 = UnityEngine.Vector3;

namespace ObjectInputHandlers
{
    public abstract class ObjectInputHandlerBase
    {
        protected bool IsLmbDown;
        protected bool IsRmbDown;
        protected bool ShouldCallDragCommand;
        protected IEditorInteractable LastHoveredObject;
        protected IEditorInteractable SelectedObject;
        protected Vector3 LastGroundPos;
        protected ObjectManager ObjectManager;
        
        
        public abstract void CleanUp(EditMode editMode);
        
        public abstract void HandleMove(IEditorInteractable editorInteractable, Vector3 groundPos);

        public abstract void HandleLmbDown();
        
        public abstract void HandleLmbUp();
        
        public abstract void HandleRmbDown();
        
        public abstract void HandleRmbUp();
    }
}