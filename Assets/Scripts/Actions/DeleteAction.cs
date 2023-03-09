using Objects;

namespace Actions
{
    public class DeleteAction : ActionBase
    {
        public ObjectBase deletedObject;
        
        public DeleteAction(ObjectBase obj)
        {
            deletedObject = obj;
        }
        public override void Execute()
        {
            deletedObject.isDeleted = true;
            deletedObject.gameObject.SetActive(false);
        }

        public override void Undo()
        {
            deletedObject.isDeleted = false;
            deletedObject.gameObject.SetActive(true);
        }
    }
}