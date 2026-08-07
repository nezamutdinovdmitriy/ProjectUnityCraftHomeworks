using Game.Scripts.GameObjects;
using UnityEngine;
using Zenject;

namespace Game.Scripts.SceneContext
{
    public class SceneContextInstaller : MonoInstaller
    {
        private readonly CharacterSystemsInstaller _characterSystemInstaller = new();

        [SerializeField]
        private Entity _character;
        
        public override void InstallBindings()
        {
            Container.Bind<CharacterProvider>().AsSingle().WithArguments(_character);
            
            Container.Install(_characterSystemInstaller);
        }
    }
}