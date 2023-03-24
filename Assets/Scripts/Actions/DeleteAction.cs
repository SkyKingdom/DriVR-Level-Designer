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
            _deletedObject.SetDeleted(true);
            _deletedObject.gameObject.SetActive(false);
        }

        public override void Undo()
        {
            _deletedObject.SetDeleted(false);
            _deletedObject.gameObject.SetActive(true);
        }
    }
}