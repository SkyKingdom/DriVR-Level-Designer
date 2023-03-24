using UnityEngine;

namespace Actions
{
    class NodeDragAction : ActionBase
    {
        private readonly Vector3 _startPosition;
        private readonly Vector3 _endPosition;
        private readonly Node _node;

        public NodeDragAction(Vector3 startPosition, Vector3 endPosition, Node node)
        {
            _startPosition = startPosition;
            _endPosition = endPosition;
            _node = node;
        }

        public override void Execute()
        {
            _node.SetPosition(_endPosition);
        }

        public override void Undo()
        {
            _node.SetPosition(_startPosition);
        }
    }
}