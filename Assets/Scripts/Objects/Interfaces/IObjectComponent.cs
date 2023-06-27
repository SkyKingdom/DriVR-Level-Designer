namespace Objects.Interfaces
{
    /// <summary>
    /// Interface for object components.<br/>
    /// Allows for the initialization of components.
    /// </summary>
    public interface IObjectComponent
    {
        public ObjectBase Owner { get; }
        
        public void Initialize(ObjectBase objectBase);
    }
}