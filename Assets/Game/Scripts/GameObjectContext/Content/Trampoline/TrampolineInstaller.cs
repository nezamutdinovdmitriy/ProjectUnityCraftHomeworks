using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
{
    public class TrampolineInstaller : MonoInstaller
    {
        [SerializeField]
        private Vector2 _force;

        [SerializeField]
        private TriggerComponent _triggerComponent;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Trampoline>()
                .AsSingle()
                .WithArguments(_force)
                .NonLazy();

            Container.Bind<TriggerComponent>().FromInstance(_triggerComponent).AsSingle();
        }
    }
}