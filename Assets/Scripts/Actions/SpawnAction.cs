using Objects;

namespace Actions
{
    class SpawnAction : ActionBase
    {
        private ObjectBase _spawnedObject;

        public SpawnAction(ObjectBase spawnedObject)
        {
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