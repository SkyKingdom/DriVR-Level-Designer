using Interfaces;
using Managers;
using UnityEngine;

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
            throw new System.NotImplementedException();
        }

        public override void HandleLmbDown()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleLmbUp()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleRmbDown()
        {
            throw new System.NotImplementedException();
        }

        public override void HandleRmbUp()
        {
            throw new System.NotImplementedException();
        }
        
        public override void CleanUp()
        {
            throw new System.NotImplementedException();
        }
    }
}