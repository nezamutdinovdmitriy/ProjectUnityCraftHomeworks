using UnityEngine;
using Zenject;

namespace Game
{
    public class TrapInstaller : MonoInstaller
    {
        [SerializeField]
        private float _maxHealth;
        
        [SerializeField]
        private int _damage;
        
        [SerializeField]
        private CollisionComponent _collisionComponent;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Trap>().AsSingle().WithArguments(_damage).NonLazy();
            
            Container.Bind<CollisionComponent>().FromInstance(_collisionComponent).AsSingle();

            Container.Bind<HealthComponent>()
                .AsSingle()
                .WithArguments(_maxHealth);
        }
    }
}