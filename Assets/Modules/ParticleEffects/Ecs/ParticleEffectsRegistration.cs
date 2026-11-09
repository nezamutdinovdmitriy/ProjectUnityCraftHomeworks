using Game.Components;
using Game.Presentation.ParticleEffects;
using Unity.Entities;
using UnityEngine;

namespace Game.View
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ParticleEffectService))]
    public class ParticleEffectsRegistration : MonoBehaviour
    {
        private World _world;
        private Entity _entity;
        private ParticleEffectsState _state;
        
        private void OnEnable()
        {
            _world = World.DefaultGameObjectInjectionWorld;

            EntityManager manager = _world.EntityManager;

            _state = new ParticleEffectsState
            {
                Service = GetComponent<ParticleEffectService>()
            };

            _entity = manager.CreateEntity();
            manager.AddComponentObject(_entity, _state);
        }
        
        private void OnDisable()
        {
            _state?.Followers.Clear();

            if (_world is {IsCreated: true} && _entity != Entity.Null)
            {
                EntityManager manager = _world.EntityManager;
                
                if (manager.Exists(_entity))
                    manager.DestroyEntity(_entity);
            }

            _entity = Entity.Null;
            _state = null;
            _world = null;
        }
    }
}