using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
{
    public class LavaInstaller : MonoInstaller
    {
        [SerializeField]
        private TriggerComponent _trigger;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Lava>()
                .AsSingle()
                .NonLazy();
            
            Container.Bind<TriggerComponent>().FromInstance(_trigger).AsSingle();
        }
    }
}