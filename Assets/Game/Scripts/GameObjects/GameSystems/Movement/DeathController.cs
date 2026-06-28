using UnityEngine;

namespace Game
{
    public class DeathController : MonoBehaviour
    {
        private HealthComponent _healthComponent;

        private DeathComponent _deathComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _deathComponent = GetComponent<DeathComponent>();
        }

        private void OnEnable() => _healthComponent.OnDied += RequestDeath;

        private void OnDisable() => _healthComponent.OnDied -= RequestDeath;

        private void RequestDeath() => _deathComponent.RequestDeath();
    }
}