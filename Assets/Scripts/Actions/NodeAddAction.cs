using System.IO;
using Objects;

namespace Actions
{
    class NodeAddAction : ActionBase
    {
        public Node Node { get; private set; }
        public PathObject Owner { get; private set; }
        
        public NodeAddAction(Node node, PathObject owner)
        {
            Node = node;
            Owner = owner;
        }
        public override void Execute()
        {
            Owner.AddPathPoint(Node);
        }

        public override void Undo()
        {
            Owner.RemovePathPoint(Node);
        }
    }
}