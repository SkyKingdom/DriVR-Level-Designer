using Objects;
using UnityEngine;

namespace Actions
{
    class ObjectTransformAction : ActionBase
    {
        private readonly TransformActionData _startTransform;
        private readonly TransformActionData _endTransform;
        private readonly ObjectBase _obj;
        public ObjectTransformAction(TransformActionData startTransform, TransformActionData endTransform, ObjectBase obj)
        {
            _startTransform = startTransform;
            _endTransform = endTransform;
            _obj = obj;
        }
        public override void Execute()
        {
            // Updates object position and rotation.
            _obj.SetPosition(_endTransform.Position);
            _obj.SetRotation(_endTransform.Rotation);
        }

        public override void Undo()
        {
            // Updates object position and rotation to the start values.
            _obj.SetPosition(_startTransform.Position);
            _obj.SetRotation(_startTransform.Rotation);
            
            // Updates handle position if object is selected.
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedObject == _obj)
                selectionManager.UpdateHandlePosition();
        }
    }

    public struct TransformActionData
    {
        public Vector3 Position;
        public Vector3 Rotation;
        
        public TransformActionData(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}