namespace Game.Scripts.Domain.App
{
    public interface IVersionProvider
    {
        public bool IsVersionValid(int version);
        public int GetNextVersion();
        public int GetCurrentVersion();
        public void IncreaseVersion();
    }
}