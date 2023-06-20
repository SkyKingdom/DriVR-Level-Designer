using Objects;

namespace Actions
{
    public class DeleteAction : ActionBase
    {
        private readonly ObjectBase _deletedObject;
        
        public DeleteAction(ObjectBase obj)
        {
            _deletedObject = obj;
        }
        public override void Execute()
        {
            // Hides object, and flags it for deletion.
            _deletedObject.SetDeleted(true);
        }

        public override void Undo()
        {
            // shows object, and removes deletion flag.
            _deletedObject.SetDeleted(false);
        }
    }
}