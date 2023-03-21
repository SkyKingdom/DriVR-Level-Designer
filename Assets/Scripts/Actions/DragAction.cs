using Objects;
using UnityEngine;

namespace Actions
{
    class DragAction : ActionBase
    {
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Transform _draggedObject;
        public DragAction(Vector3 startPosition, Vector3 endPosition, Transform draggedObject)
        {
            _startPosition = startPosition;
            _endPosition = endPosition;
            _draggedObject = draggedObject;
        }
        public override void Execute()
        {
            _draggedObject.GetComponent<ObjectBase>().OnReposition();
        }

        public override void Undo()
        {
            _draggedObject.position = _startPosition;
            _draggedObject.GetComponent<ObjectBase>().OnReposition();
        }
    }
}