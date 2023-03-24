namespace Objects.Interfaces
{
    public interface IObjectComponent
    {
        public ObjectBase Owner { get; }
        
        public void Initialize(ObjectBase objectBase);
    }
}