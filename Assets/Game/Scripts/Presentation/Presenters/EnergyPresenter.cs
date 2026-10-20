using Game.Components;
using SampleGame;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game.View.Presenters
{
    public class EnergyPresenter : MonoBehaviour
    {
        [SerializeField] private TextView _view;

        private EntityManager _entityManager;
        private EntityQuery _query;

        private Entity _player;

        private World _world;

        private void Awake()
        {
            _world = World.DefaultGameObjectInjectionWorld;
            
            if (_world == null || _world.IsCreated == false)
                return;
            
            _entityManager = _world.EntityManager;

            _player = Entity.Null;

            _query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Player, LocalPlayer, Energy>()
                .Build(_entityManager);
        }

        private void OnDestroy()
        {
            if (_world is {IsCreated: true})
                _query.Dispose();
        }

        private void LateUpdate()
        {
            if (_world == null || _world.IsCreated == false)
                return;
            
            if (_entityManager.Exists(_player) == false)
            {
                if (_query.IsEmpty)
                    return;

                _player = _query.GetSingletonEntity();
            }

            int energy = _entityManager.GetComponentData<Energy>(_player).Value;
            _view.SetText(energy.ToString());
        }
    }
}