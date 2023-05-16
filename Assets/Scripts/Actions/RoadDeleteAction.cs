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
            _index = RoadTool.Instance.RemovePointTemporarily(_roadPoint);
            _roadPoint.gameObject.SetActive(false);
        }

        public override void Undo()
        {
            RoadTool.Instance.AddPoint(_index, _roadPoint);
            _roadPoint.gameObject.SetActive(true);
        }
    }
}