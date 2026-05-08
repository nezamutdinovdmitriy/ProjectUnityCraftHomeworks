using Modules.Utils;
using UnityEngine;

namespace Game
{
    public class PlayerEffectsController : MonoBehaviour
    {
        [SerializeField]
        private PlayerShip _player;

        [SerializeField]
        private CameraShaker _cameraShaker;

        private void OnEnable() => _player.HealthComponent.Changed += OnHealthChanged;
        private void OnDisable() => _player.HealthComponent.Changed -= OnHealthChanged;

        private void OnHealthChanged(int obj) => _cameraShaker.Shake();
    }
}