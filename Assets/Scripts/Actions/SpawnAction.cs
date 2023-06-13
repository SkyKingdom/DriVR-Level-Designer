using Managers;
using Objects;
using UnityEngine;

namespace Actions
{
    class SpawnAction : ActionBase
    {
        private readonly ObjectInspector _objectInspector;
        private ObjectBase _spawnedObject;

        public SpawnAction(ObjectInspector objectInspector, ObjectBase spawnedObject)
        {
            _objectInspector = objectInspector;
            _spawnedObject = spawnedObject;
        }
        public override void Execute()
        {

        }

        public override void Undo()
        {
            // if (_objectInspector.SelectedObject == _spawnedObject)
            // {
            //     _objectInspector.DeselectObject();
            // }
            
            _spawnedObject.Delete();
        }
    }
}