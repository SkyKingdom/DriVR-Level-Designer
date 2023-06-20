using Vector3 = UnityEngine.Vector3;

namespace Actions
{
    class RoadAddAction : ActionBase
    {
        private readonly Vector3 _position;
        private RoadPoint point;
        
        public RoadAddAction(Vector3 position)
        {
            _position = position;
        }
        
        public override void Execute()
        {
            // Instantiates and stores a reference to the new road point.
            point = RoadTool.Instance.AddPoint(_position);
        }

        public override void Undo()
        {
            // If the road point is selected, deselect it.
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedRoadPoint == point.owner)
                selectionManager.DeselectRoadPoint();
            
            // Destroys the road point.
            RoadTool.Instance.RemovePoint(point);
        }
    }
}