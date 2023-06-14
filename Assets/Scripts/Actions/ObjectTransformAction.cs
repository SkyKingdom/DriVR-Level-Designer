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
            _obj.OnReposition(_endTransform.Position);
            _obj.SetRotation(_endTransform.Rotation);
        }

        public override void Undo()
        {
            _obj.OnReposition(_startTransform.Position);
            _obj.SetRotation(_startTransform.Rotation);
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