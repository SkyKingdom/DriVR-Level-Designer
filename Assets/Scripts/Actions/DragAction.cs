using Objects;
using UnityEngine;

namespace Actions
{
    class DragAction : ActionBase
    {
        private readonly Vector3 _startPosition;
        private readonly Vector3 _endPosition;
        private readonly ObjectBase _draggedObject;
        public DragAction(Vector3 startPosition, Vector3 endPosition, ObjectBase draggedObject)
        {
            _startPosition = startPosition;
            _endPosition = endPosition;
            _draggedObject = draggedObject;
        }
        public override void Execute()
        {
            _draggedObject.OnReposition(_endPosition);
        }

        public override void Undo()
        {
            _draggedObject.OnReposition(_startPosition);
        }
    }
}