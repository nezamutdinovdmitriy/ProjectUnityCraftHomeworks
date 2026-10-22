using System;
using Modules.AudioEvents;
using Unity.Entities;
using UnityEngine;

namespace Game.Presentation.Audio.Ecs
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSystem))]
    public sealed class AudioPlaybackRegistration : MonoBehaviour
    {
        private World _world;
        private Entity _entity = Entity.Null;
        
        public AudioPlayback Playback { get; private set; }

        private void OnEnable()
        {
            _world = World.DefaultGameObjectInjectionWorld;
            EntityManager manager = _world.EntityManager;

            using EntityQuery existing = manager.CreateEntityQuery(
                ComponentType.ReadOnly<AudioPlaybackState>());

            AudioSystem audioSystem = GetComponent<AudioSystem>();

            Playback = new AudioPlayback(audioSystem);

            _entity = manager.CreateEntity();

            manager.AddComponentObject(
                _entity,
                new AudioPlaybackState
                {
                    Playback = Playback
                });
        }
        
        private void OnDisable()
        {
            if (_world is {IsCreated: true}
                && _entity != Entity.Null)
            {
                EntityManager manager = _world.EntityManager;

                if (manager.Exists(_entity))
                    manager.DestroyEntity(_entity);
            }

            _entity = Entity.Null;
            _world = null;
            Playback = null;
        }
    }
}