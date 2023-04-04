using SplineMesh;
using UnityEngine;

namespace Actions
{
    class RoadPointAction : ActionBase
    {
        private readonly RoadTool _roadTool;
        private readonly SplineNode _node;
        
        public RoadPointAction(RoadTool roadTool, Vector3 position)
        {
            _roadTool = roadTool;
            _node = new SplineNode(position, Vector3.zero);
            
        }

        public override void Execute()
        {
            _roadTool.AddPoint(_node);
        }

        public override void Undo()
        {
            _roadTool.RemovePoint(_node);
        }
    }
}