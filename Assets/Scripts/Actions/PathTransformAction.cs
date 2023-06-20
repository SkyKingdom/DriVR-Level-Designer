using UnityEngine;

namespace Actions
{
    class PathTransformAction : ActionBase
    {
        private readonly TransformActionData _startTransform;
        private readonly TransformActionData _endTransform;
        private readonly PathPoint _pathPoint;

        public PathTransformAction(TransformActionData startTransform, TransformActionData endTransform, PathPoint pathPoint)
        {
            _startTransform = startTransform;
            _endTransform = endTransform;
            _pathPoint = pathPoint;
        }

        public override void Execute()
        {
            // Updates path point position.
            _pathPoint.SetPosition(_endTransform.Position);
        }

        public override void Undo()
        {
            // Updates path point position to the start value.
            _pathPoint.SetPosition(_startTransform.Position);
            
            // Updates handle position if path point is selected.
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedPathPoint == _pathPoint.Container)
                selectionManager.UpdateHandlePosition();
        }
    }
}