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
            point = RoadTool.Instance.AddPoint(_position);
        }

        public override void Undo()
        {
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedRoadPoint == point.owner)
                selectionManager.DeselectRoadPoint();
            RoadTool.Instance.RemovePoint(point);
        }
    }
}