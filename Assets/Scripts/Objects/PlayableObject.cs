namespace Objects
{
    class PlayableObject : PathObject
    {
        
        public float SwitchViewTime { get; private set; }
        
        public void SetViewValues(float switchViewTime)
        {
            SwitchViewTime = switchViewTime;
        }
    }
}