using UnityEngine;

namespace Actions
{
    class NodeDragAction : ActionBase
    {
        public Vector3 StartPosition { get; private set; }
        public Vector3 EndPosition { get; private set; }
        public Node Node { get; private set; }

        public NodeDragAction(Vector3 startPosition, Vector3 endPosition, Node node)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            Node = node;
        }

        public override void Execute()
        {
            Node.SetPosition(EndPosition);
            Node.ReportPositionChange();
        }

        public override void Undo()
        {
            Node.SetPosition(StartPosition);
            Node.ReportPositionChange();
        }
    }
}