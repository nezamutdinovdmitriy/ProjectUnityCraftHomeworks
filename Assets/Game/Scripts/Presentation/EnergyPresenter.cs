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
        
        private EntityQuery _query;
        private World _world;
        
        private void Awake()
        {
            _world = World.DefaultGameObjectInjectionWorld;
            
            if (_world == null || _world.IsCreated == false)
                return;

            _query = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Player, LocalPlayer, Energy>()
                .Build(_world.EntityManager);
        }

        private void OnDestroy()
        {
            if (_world is {IsCreated: true})
                _query.Dispose();
        }

        private void LateUpdate()
        {
            if (_world == null || 
                _world.IsCreated == false ||
                !_query.TryGetSingleton(out Energy energy))
                return;

            _view.SetText(energy.Value.ToString());
        }
    }
}