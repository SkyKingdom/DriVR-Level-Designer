using UnityEngine;

namespace Actions
{
    class RotateAction : ActionBase
    {
        public Transform ObjectToRotate;
        public Vector3 InitialRotation;

        public RotateAction(Transform objectToRotate, Vector3 initialRotation)
        {
            ObjectToRotate = objectToRotate;
            InitialRotation = initialRotation;
        }
        
        public override void Execute()
        {
        }

        public override void Undo()
        {
            ObjectToRotate.rotation = Quaternion.Euler(InitialRotation);
        }
    }
}