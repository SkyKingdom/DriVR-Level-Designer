using Interfaces;
using UnityEngine;

namespace ObjectInputHandlers
{
    class NoEditInputHandler : ObjectInputHandlerBase
    {
        public override void CleanUp(EditMode editMode)
        {
        }

        public override void HandleMove(IEditorInteractable editorInteractable, Vector3 groundPos)
        {
        }

        public override void HandleLmbDown()
        {
        }

        public override void HandleLmbUp()
        {
        }

        public override void HandleRmbDown()
        {
        }

        public override void HandleRmbUp()
        {
        }
    }
}