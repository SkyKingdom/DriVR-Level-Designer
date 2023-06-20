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
            // If the path point is selected, deselect it.
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedPathPoint == _pathPoint.Container)
                selectionManager.DeselectPathPoint();
            
            // Destroys the path point temporarily.
            index = _pathPoint.Owner.Path.RemovePathTemporarily(_pathPoint);
            // Hide the path point.
            _pathPoint.Container.gameObject.SetActive(false);
        }

        public override void Undo()
        {
            // Adds the path point back to the path.
            _pathPoint.Owner.Path.AddPathPoint(_pathPoint, index);
            
            // Shows the path point.
            _pathPoint.Container.gameObject.SetActive(true);
        }
    }
}