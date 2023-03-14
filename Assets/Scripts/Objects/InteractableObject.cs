namespace Objects
{
    class InteractableObject : PathObject
    {
        public bool Answer { get; private set; }
        
        public bool AlwaysInteractable { get; private set; }
        
        public float InteractionStartTime { get; private set; }
        public float InteractionEndTime { get; private set; }

        public void SetInteractionValues(bool answer, bool alwaysInteractable)
        {
            Answer = answer;
            AlwaysInteractable = alwaysInteractable;
        }
        
        public void SetInteractionValues(bool answer, float interactionStartTime, float interactionEndTime)
        {
            Answer = answer;
            InteractionStartTime = interactionStartTime;
            InteractionEndTime = interactionEndTime;
        }
    }
}