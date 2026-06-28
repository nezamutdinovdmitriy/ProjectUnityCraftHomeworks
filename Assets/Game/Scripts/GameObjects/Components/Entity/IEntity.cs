namespace Game.Scripts.GameObjects
{
    public interface IEntity
    {
        public string Name { get; set; }
        public T Get<T>() where T : class;
        public bool TryGet<T>(out T result) where T : class;
    }
}