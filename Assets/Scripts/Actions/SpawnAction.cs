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
            //_settingsManager.SelectObject(_spawnedObject);
            InputManager.Instance.SelectObject(_spawnedObject);
        }

        public override void Undo()
        {
            if (_objectInspector.SelectedObject == _spawnedObject)
            {
                _objectInspector.DeselectObject();
            }
            InputManager.Instance.DropObject(_spawnedObject);
            
            _spawnedObject.Delete();
        }
    }
}