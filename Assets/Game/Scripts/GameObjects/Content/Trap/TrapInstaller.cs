using UnityEngine;
using Zenject;

namespace Game
{
    public class TrapInstaller : MonoInstaller
    {
        [SerializeField]
        private CollisionComponent _collisionComponent;

        [SerializeField]
        private int _damage;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Trap>().AsSingle().WithArguments(_damage).NonLazy();
            
            Container.Bind<CollisionComponent>().FromInstance(_collisionComponent).AsSingle();
        }
    }
}