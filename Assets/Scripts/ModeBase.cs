
public abstract class ModeBase
{
    
    public abstract void OnEnter();

    public abstract void OnExit();
}

public class ViewMode : ModeBase
{
    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
}