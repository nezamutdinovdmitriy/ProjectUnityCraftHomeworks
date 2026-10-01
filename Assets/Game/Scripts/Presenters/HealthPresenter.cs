using Game.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Health = Game.Components.Health;
using MaxHealth = Game.Components.MaxHealth;
using Team = Game.Components.Team;
using TeamType = Game.Components.TeamType;
using HealthView = SampleGame.HealthView;

namespace Game.View.Presenters
{
    public sealed class HealthPresenter : MonoBehaviour
    {
        [SerializeField] private HealthView _view;
        [SerializeField] private TeamType _team;

        private World _world;
        private EntityManager _entityManager;
        private EntityQuery _basesQuery;

        private bool _initialized;
        private bool _hasDisplayedHealth;

        private float _lastHealth;
        private float _lastMaxHealth;

        private void LateUpdate()
        {
            if (_initialized == false)
            {
                if (TryInitialize() == false)
                    return;
            }

            if (_world.IsCreated == false)
                return;

            using NativeArray<Entity> bases =
                _basesQuery.ToEntityArray(Allocator.Temp);

            foreach (Entity entity in bases)
            {
                Team team = _entityManager.GetComponentData<Team>(entity);

                if (team.Value == _team)
                {
                    float health = _entityManager.GetComponentData<Health>(entity).Value;
                    float maxHealth = _entityManager.GetComponentData<MaxHealth>(entity).Value;

                    if (maxHealth <= 0f)
                        return;

                    if (_hasDisplayedHealth
                        && Mathf.Approximately(health, _lastHealth)
                        && Mathf.Approximately(maxHealth, _lastMaxHealth))
                        return;

                    _view.HealthText = $"{health} / {maxHealth}";

                    _view.HealthProgress = Mathf.Clamp01(health / maxHealth);

                    _lastHealth = health;
                    _lastMaxHealth = maxHealth;
                    _hasDisplayedHealth = true;

                    return;
                }
            }
        }

        private bool TryInitialize()
        {
            _world = World.DefaultGameObjectInjectionWorld;

            if (_world == null || _world.IsCreated == false)
                return false;

            _entityManager = _world.EntityManager;

            _basesQuery = _entityManager
                .CreateEntityQuery(
                    ComponentType.ReadOnly<Base>(),
                    ComponentType.ReadOnly<Team>(),
                    ComponentType.ReadOnly<Health>(),
                    ComponentType.ReadOnly<MaxHealth>());

            _initialized = true;
            return true;
        }

        private void OnDestroy()
        {
            if (_initialized == false)
                return;

            if (_world.IsCreated)
                _basesQuery.Dispose();
        }
    }
}