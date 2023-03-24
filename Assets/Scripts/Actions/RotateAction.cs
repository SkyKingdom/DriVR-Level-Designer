using Objects;
using UnityEngine;

namespace Actions
{
    class RotateAction : ActionBase
    {
        private readonly ObjectBase _rotatedObject;
        private readonly Vector3 _initialRotation;

        public RotateAction(ObjectBase rotatedObject, Vector3 initialRotation)
        {
            _rotatedObject = rotatedObject;
            _initialRotation = initialRotation;
        }
        
        public override void Execute()
        {
            
        }

        public override void Undo()
        {
            _rotatedObject.SetRotation(_initialRotation);
        }
    }
}