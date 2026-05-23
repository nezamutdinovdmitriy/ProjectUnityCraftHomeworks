using Extensions;
using UnityEngine;
using Zenject;

namespace GameSystems.PlayerContext
{
    public class PlayerContextInstaller : MonoInstaller
    {
        [SerializeField]
        private PlayerInputInstaller _playerInputInstaller;

        [SerializeField]
        private CharacterInstaller _characterInstaller;

        public override void InstallBindings()
        {
            Container
                .Install(_playerInputInstaller)
                .Install(_characterInstaller);
        }
    }
}