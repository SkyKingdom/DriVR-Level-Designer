namespace Actions
{
    class PathDeleteAction : ActionBase
    {
        private Node _node;
        private int index;
        
        public PathDeleteAction(Node node)
        {
            _node = node;
        }

        public override void Execute()
        {
            index = _node.Owner.Path.RemovePathTemporarily(_node);
            _node.Container.gameObject.SetActive(false);
        }

        public override void Undo()
        {
            _node.Owner.Path.AddPathPoint(_node, index);
            _node.Container.gameObject.SetActive(true);
        }
    }
}