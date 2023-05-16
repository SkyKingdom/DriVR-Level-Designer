using UnityEngine;

namespace Actions
{
    class RoadDragAction : ActionBase
    {
        private readonly Vector3 _startPosition;
        private readonly Vector3 _endPosition;
        private readonly RoadPoint _road;
        
        public RoadDragAction(Vector3 startPosition, Vector3 endPosition, RoadPoint road)
        {
            _startPosition = startPosition;
            _endPosition = endPosition;
            _road = road;
        }
        
        public override void Execute()
        {
            _road.position = _endPosition;
            RoadTool.Instance.UpdateRoad();
        }

        public override void Undo()
        {
            _road.position = _startPosition;
            _road.owner.transform.position = _startPosition;
            RoadTool.Instance.UpdateRoad();
        }
    }
}