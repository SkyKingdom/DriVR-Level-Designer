using Vector3 = UnityEngine.Vector3;

namespace Actions
{
    class RoadPointAction : ActionBase
    {
        private readonly Vector3 _position;
        private RoadPoint point;
        
        public RoadPointAction(Vector3 position)
        {
            _position = position;
        }
        
        public override void Execute()
        {
            point = RoadTool.Instance.AddPoint(_position);
        }

        public override void Undo()
        {
            RoadTool.Instance.RemovePoint(point);
        }
    }
}