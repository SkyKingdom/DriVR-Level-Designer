using Objects;

namespace Actions
{
    class PathAddAction : ActionBase
    {
        private readonly PathPoint _pathPoint;
        private readonly ObjectBase _owner;
        
        public PathAddAction(PathPoint pathPoint, ObjectBase owner)
        {
            _pathPoint = pathPoint;
            _owner = owner;
        }
        public override void Execute()
        {
            _owner.Path.AddPathPoint(_pathPoint);
        }

        public override void Undo()
        {
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedPathPoint == _pathPoint.Container)
                selectionManager.DeselectPathPoint();
            
            _owner.Path.RemovePathPoint(_pathPoint);
        }
    }
}