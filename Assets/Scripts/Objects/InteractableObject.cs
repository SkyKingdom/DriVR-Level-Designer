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
        
        public float interactionStartTime;
        public float interactionEndTime;
        
        public void SetInteractionStartTime(float time)
        {
            interactionStartTime = time;
        }
        
        public void SetInteractionStartTime(string time)
        {
            interactionStartTime = float.Parse(time);
        }
        
        public void SetInteractionEndTime(float time)
        {
            interactionEndTime = time;
        }
        
        public void SetInteractionEndTime(string time)
        {
            interactionEndTime = float.Parse(time);
        }
    }
}