namespace Objects.Interfaces
{
    public interface IPlayable
    {
        public float SwitchTime { get; }
        public bool PlayOnStart { get; }
        
        public void SetSwitchTime(float switchTime);
        public void SetPlayOnStart(bool playOnStart);
    }
}