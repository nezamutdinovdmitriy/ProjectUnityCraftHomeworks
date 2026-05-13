using Modules.Utils;
using UnityEngine;

namespace Game
{
    public class CameraShakeController : MonoBehaviour
    {
        [SerializeField]
        private Ship _ship;

        [SerializeField]
        private CameraShaker _cameraShaker;

        private void OnEnable() => _ship.HealthComponent.Changed += OnHealthChanged;
        private void OnDisable() => _ship.HealthComponent.Changed -= OnHealthChanged;

        private void OnHealthChanged(int value) => _cameraShaker.Shake();
    }
}