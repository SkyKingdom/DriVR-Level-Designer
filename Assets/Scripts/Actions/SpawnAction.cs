using Objects;
using UnityEngine;

namespace Actions
{
    class SpawnAction : ActionBase
    {
        private readonly ObjectManager _objectManager;
        private readonly SettingsManager _settingsManager;
        private ObjectBase _spawnedObject;
        private readonly Vector3 _position;

        public SpawnAction(ObjectManager objectManager, SettingsManager settingsManager, Vector3 position)
        {
            _objectManager = objectManager;
            _settingsManager = settingsManager;
            _position = position;
        }
        public override void Execute()
        {
            _spawnedObject = _objectManager.Spawn(_position);
        }

        public override void Undo()
        {
            if (_settingsManager.SelectedObject == _spawnedObject)
            {
                _settingsManager.DeselectObject();
            }
            
            _spawnedObject.Delete();
        }
    }
}