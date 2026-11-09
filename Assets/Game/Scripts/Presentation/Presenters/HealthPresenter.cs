using System;
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
        [SerializeField] private HealthView _blueTeam;
        [SerializeField] private HealthView _redTeam;
        
        private EntityQuery _query;
        private World _world;
        
        private void Awake()
        {
            _world = World.DefaultGameObjectInjectionWorld;

            _query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Base, Team, Health, MaxHealth>()
                .Build(_world.EntityManager);
        }

        private void OnDestroy()
        {
            if (_world is not {IsCreated: true})
                return;
            
            _query.Dispose();
        }

        private void LateUpdate()
        {
            if (_world is not {IsCreated: true})
                return;
            
            NativeArray<Team> teams = _query.ToComponentDataArray<Team>(Allocator.Temp);
            NativeArray<MaxHealth> maxHealths = _query.ToComponentDataArray<MaxHealth>(Allocator.Temp);
            NativeArray<Health> healths = _query.ToComponentDataArray<Health>(Allocator.Temp);

            for (int i = 0; i < teams.Length; i++)
            {
                HealthView view = teams[i].Value switch
                {
                    TeamType.Blue => _blueTeam,
                    TeamType.Red => _redTeam,
                    _ => null
                };

                float maxHealth = maxHealths[i].Value;
                
                if(view == null || maxHealth <= 0)
                    continue;

                float health = healths[i].Value;
                
                view.HealthText = $"{health} / {maxHealth}";
                view.HealthProgress = Mathf.Clamp01(health / maxHealth);
            }
        }
    }
}