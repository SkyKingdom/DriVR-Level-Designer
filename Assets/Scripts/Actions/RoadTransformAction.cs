namespace Actions
{
    class RoadTransformAction : ActionBase
    {
        private readonly TransformActionData _startTransform;
        private readonly TransformActionData _endTransform;
        private readonly RoadPoint _road;
        
        public RoadTransformAction(TransformActionData startTransform, TransformActionData endTransform, RoadPoint road)
        {
            _startTransform = startTransform;
            _endTransform = endTransform;
            _road = road;
        }
        
        public override void Execute()
        {
            _road.SetPosition(_endTransform.Position);
            RoadTool.Instance.UpdateRoad();
        }

        public override void Undo()
        {
            _road.SetPosition(_startTransform.Position);
            RoadTool.Instance.UpdateRoad();
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedRoadPoint == _road.owner)
                selectionManager.UpdateHandlePosition();
        }
    }
}