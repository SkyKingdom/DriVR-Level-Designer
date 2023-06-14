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
            var selectionManager = DesignerManager.Instance.SelectionManager;
            if (selectionManager.SelectedObject == _spawnedObject)
                selectionManager.DeselectObject();
            
            _spawnedObject.Delete();
        }
    }
}