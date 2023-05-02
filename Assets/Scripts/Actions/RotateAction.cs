using Objects;
using UnityEngine;

namespace Actions
{
    class RotateAction : ActionBase
    {
        private readonly ObjectBase _rotatedObject;
        private readonly Vector3 _initialRotation;
        private readonly Vector3 _finalRotation;

        public RotateAction(Vector3 initialRotation, Vector3 finalRotation, ObjectBase rotatedObject)
        {
            _rotatedObject = rotatedObject;
            _initialRotation = initialRotation;
            _finalRotation = finalRotation;
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