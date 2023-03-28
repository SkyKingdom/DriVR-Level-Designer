namespace Objects.Interfaces
{
    public interface IInteractable
    {
        public bool Answer { get;}
        public bool AlwaysInteractable { get; }
        public float InteractionStartTime { get; }
        public float InteractionEndTime { get; }
        
        public void SetInteractionValues(bool answer, float interactionStartTime, float interactionEndTime);
        public void SetAlwaysInteractable(bool alwaysInteractable, bool answer);
    }
}
