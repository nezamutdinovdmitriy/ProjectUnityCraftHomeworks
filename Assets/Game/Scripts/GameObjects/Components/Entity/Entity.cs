using Zenject;

namespace Game.Scripts.GameObjects
{
    public class Entity : GameObjectContext, IEntity
    {
        public string Name
        {
            get => name;
            set => name = value;
        }
        
        public T Get<T>() where T : class => Container.Resolve<T>();

        public bool TryGet<T>(out T result) where T : class
        {
            result = Container.TryResolve<T>();
            return result != null;
        }
    }
}