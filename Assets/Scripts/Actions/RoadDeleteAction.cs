namespace Actions
{
    class RoadDeleteAction : ActionBase
    {
        private RoadPoint _roadPoint;
        private int _index;
        
        public RoadDeleteAction(RoadPoint roadPoint)
        {
            _roadPoint = roadPoint;
        }
        
        public override void Execute()
        {
            // If the road point is selected, deselect it.
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedRoadPoint == _roadPoint.owner)
                selectionManager.DeselectRoadPoint();
            
            // Destroys the road point temporarily.
            _index = RoadTool.Instance.RemovePointTemporarily(_roadPoint);
            
            // Hide the road point.
            _roadPoint.gameObject.SetActive(false);
        }

        public override void Undo()
        {
            // Adds the road point back to the road.
            RoadTool.Instance.AddPoint(_index, _roadPoint);
            
            // Shows the road point.
            _roadPoint.gameObject.SetActive(true);
        }
    }
}