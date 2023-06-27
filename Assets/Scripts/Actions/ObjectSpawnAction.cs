using Objects;
using UnityEngine;

namespace Actions
{
    class ObjectSpawnAction : ActionBase
    {
        private ObjectBase _spawnedObject;
        private readonly ObjectBase _prefabToSpawn;
        private readonly Vector3 _spawnPosition;

        public ObjectSpawnAction(ObjectBase objectPrefab, Vector3 spawnPosition)
        {
            _prefabToSpawn = objectPrefab;
            _spawnPosition = spawnPosition;
        }
        public override void Execute()
        {
            // Instantiates and stores a reference to the new object.
            _spawnedObject = Object.Instantiate(_prefabToSpawn, _spawnPosition, Quaternion.identity);
            
            // Initializes the object.
            _spawnedObject.Initialize(_spawnedObject.gameObject.name, _prefabToSpawn.name, false);
        }

        public override void Undo()
        {
            // If the object is selected, deselect it.
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedObject == _spawnedObject || selectionManager.LastSelectedObject == _spawnedObject)
                selectionManager.DeselectObject();
            
            // Destroys the object.
            _spawnedObject.Delete();
        }
    }
}