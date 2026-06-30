using UnityEngine;
using Zenject;

namespace Game
{
    public class LavaInstaller : MonoInstaller
    {
        [SerializeField]
        private Lava _lava;
        
        [SerializeField]
        private TriggerComponent _trigger;
        
        public override void InstallBindings()
        {
            Container.Bind<Lava>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
            
            Container.Bind<TriggerComponent>().FromInstance(_trigger).AsSingle();
        }
    }
}