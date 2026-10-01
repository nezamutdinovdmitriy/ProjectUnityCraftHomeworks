using Game.Components;
using SampleGame;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using TeamType = Game.Components.TeamType;

namespace Game.View.Presenters
{
    public sealed class HealthPresenter : MonoBehaviour
    {
        [SerializeField] private HealthView _view;
        [SerializeField] private TeamType _team;

        private EntityQuery _query;
        private EntityManager _entityManager;

        private void Awake()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            _query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Base, Team, Health, MaxHealth>()
                .Build(_entityManager);
        }

        private void LateUpdate()
        {
            if (_view == null
                || _entityManager.World is not {IsCreated: true})
                return;

            NativeArray<Entity> bases = _query.ToEntityArray(Allocator.Temp);

            foreach (Entity entity in bases)
            {
                Team teamComponent = _entityManager.GetComponentData<Team>(entity);
                float maxHealth = _entityManager.GetComponentData<MaxHealth>(entity).Value;

                if (teamComponent.Value != _team 
                    || maxHealth <= 0f)
                    continue;

                float health = _entityManager.GetComponentData<Health>(entity).Value;

                _view.HealthText = $"{health} / {maxHealth}";
                _view.HealthProgress = Mathf.Clamp01(health / maxHealth);

                return;
            }
        }
    }
}