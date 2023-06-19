namespace Actions
{
    class PathDeleteAction : ActionBase
    {
        private PathPoint _pathPoint;
        private int index;
        
        public PathDeleteAction(PathPoint pathPoint)
        {
            _pathPoint = pathPoint;
        }

        public override void Execute()
        {
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedPathPoint == _pathPoint.Container)
                selectionManager.DeselectPathPoint();
            index = _pathPoint.Owner.Path.RemovePathTemporarily(_pathPoint);
            _pathPoint.Container.gameObject.SetActive(false);
        }

        public override void Undo()
        {
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedPathPoint == _pathPoint.Container)
                selectionManager.DeselectPathPoint();
            _pathPoint.Owner.Path.AddPathPoint(_pathPoint, index);
            _pathPoint.Container.gameObject.SetActive(true);
        }
    }
}