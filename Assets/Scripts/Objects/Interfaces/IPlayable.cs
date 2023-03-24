namespace Objects.Interfaces
{
    public interface IPlayable
    {
        public float SwitchViewTime { get; }
        public bool PlayOnStart { get; }
        
        public void SetViewValues(float switchViewTime);
        public void SetPlayOnStart(bool playOnStart);
    }
}