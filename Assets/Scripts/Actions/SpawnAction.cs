using Objects;
using UnityEngine;

namespace Actions
{
    class SpawnAction : ActionBase
    {
        private ObjectManager _objectManager;
        private SettingsManager _settingsManager;
        private ObjectBase _spawnedObject;
        private Vector3 _position;

        public SpawnAction(ObjectManager objectManager, SettingsManager settingsManager, Vector3 position)
        {
            _objectManager = objectManager;
            _settingsManager = settingsManager;
            _position = position;
        }
        public override void Execute()
        {
            _spawnedObject = _objectManager.Spawn(_position);
            _spawnedObject.OnSpawn();
        }

        public override void Undo()
        {
            if (_settingsManager.SelectedObject == _spawnedObject)
            {
                _settingsManager.DeselectObject();
            }
            
            var pathObject = _spawnedObject as PathObject;

            if (pathObject)
            {
                foreach (var p in pathObject.PathPoints)
                {
                    p.GameObject.Destroy();
                }
            }
            
            _spawnedObject.gameObject.Destroy();
        }
    }
}