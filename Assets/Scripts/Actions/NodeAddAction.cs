using Objects;

namespace Actions
{
    class NodeAddAction : ActionBase
    {
        private readonly Node _node;
        private readonly ObjectBase _owner;
        
        public NodeAddAction(Node node, ObjectBase owner)
        {
            _node = node;
            _owner = owner;
        }
        public override void Execute()
        {
            _owner.Path.AddPathPoint(_node);
        }

        public override void Undo()
        {
            _owner.Path.RemovePathPoint(_node);
        }
    }
}