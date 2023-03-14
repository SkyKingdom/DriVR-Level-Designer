namespace Objects
{
    public enum Answer
    {
        Correct,
        Incorrect
    }
    class InteractableObject : PathObject
    {
        public Answer answer;

        public float Speed { get; private set; }
        
        public float AnimationStartTime { get; private set; }
        public float InteractionStartTime { get; private set; }
        public float InteractionEndTime { get; private set; }
        
        public void SetValues(float speed, float animationStartTime, float interactionStartTime, float interactionEndTime)
        {
            Speed = speed;
            AnimationStartTime = animationStartTime;
            InteractionStartTime = interactionStartTime;
            InteractionEndTime = interactionEndTime;
        }

    }
}