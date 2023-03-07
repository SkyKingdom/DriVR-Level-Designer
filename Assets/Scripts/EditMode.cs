public class EditMode : ModeBase
{
    private ObjectManager _manager;
    public EditMode(ObjectManager manager)
    {
        _manager = manager;
    }
    
    public override void OnEnter()
    {
        _manager.enabled = true;
    }
    
    public override void OnExit()
    {
        _manager.enabled = false;
    }
}