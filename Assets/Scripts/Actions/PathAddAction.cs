using Objects;
using UnityEngine;

namespace Actions
{
    class PathAddAction : ActionBase
    {
        private PathPoint _pathPoint;
        private ObjectBase _owner;
        private readonly Vector3 _position;
        
        public PathAddAction(Vector3 pos)
        {
            _position = pos;
        }
        public override void Execute()
        {
            // Creates a new path point and adds it to the path.
            _pathPoint = DesignerManager.Instance.PathManager.InstantiatePathPoint(_position);
            
            // Stores a reference to the path owner.
            _owner = _pathPoint.Owner;
            
            // Adds the path point to the owner's path.
            _owner.Path.AddPathPoint(_pathPoint);
            
            // Highlights the path.
            _owner.Path.HighlightPath(null);
        }

        public override void Undo()
        {
            // If the path point is selected, deselect it.
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedPathPoint == _pathPoint.Container)
                selectionManager.DeselectPathPoint();
            
            // Destroys the path point.
            _owner.Path.RemovePathPoint(_pathPoint);
        }
    }
}