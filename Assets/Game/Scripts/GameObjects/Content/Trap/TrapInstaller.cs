using UnityEngine;
using Zenject;

namespace Game
{
    public class TrapInstaller : MonoInstaller
    {
        [SerializeField]
        private CollisionComponent _collisionComponent;

        [SerializeField]
        private Trap _trap;
        
        public override void InstallBindings()
        {
            Container.Bind<CollisionComponent>().FromInstance(_collisionComponent).AsSingle();
        }
    }
}