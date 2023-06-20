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
            // Updates road point position.
            _road.SetPosition(_endTransform.Position);
                
            // Updates road mesh.
            RoadTool.Instance.UpdateRoad();
        }

        public override void Undo()
        {
            // Updates road point position to the start value.
            _road.SetPosition(_startTransform.Position);
            
            // Updates road mesh.
            RoadTool.Instance.UpdateRoad();
            
            // Updates handle position if road point is selected.
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedRoadPoint == _road.owner)
                selectionManager.UpdateHandlePosition();
        }
    }
}