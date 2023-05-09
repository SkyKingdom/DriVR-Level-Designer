using Objects;
using UnityEngine;

namespace Actions
{
    class SpawnAction : ActionBase
    {
        private readonly SettingsManager _settingsManager;
        private ObjectBase _spawnedObject;

        public SpawnAction(SettingsManager settingsManager, ObjectBase spawnedObject)
        {
            _settingsManager = settingsManager;
            _spawnedObject = spawnedObject;
        }
        public override void Execute()
        {
            //_settingsManager.SelectObject(_spawnedObject);
            InputHandler.Instance.SelectObject(_spawnedObject);
        }

        public override void Undo()
        {
            if (_settingsManager.SelectedObject == _spawnedObject)
            {
                _settingsManager.DeselectObject();
            }
            InputHandler.Instance.DropObject(_spawnedObject);
            
            _spawnedObject.Delete();
        }
    }
}