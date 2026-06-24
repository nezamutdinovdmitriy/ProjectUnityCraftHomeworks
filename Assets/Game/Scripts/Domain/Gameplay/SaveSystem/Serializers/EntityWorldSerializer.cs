using System;
using System.Collections.Generic;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using SampleGame.Common;
using SampleGame.Gameplay;
using UnityEngine;

namespace Game.Scripts.Domain.Serializers
{
    [Serializable]
    public struct EntityData
    {
        public int Id;
        public string Name;
        
        public SerializedVector3 Position;
        public SerializedVector3 Rotation;

        public Dictionary<string, JToken> Components;
    }
    
    public class EntityWorldSerializer : ISaveSerializer<EntityData[]>
    {
        private readonly List<ISerializableComponent> _componentsCache;
        private readonly ComponentSerializer _componentSerializer;
        
        private readonly EntityWorld _entityWorld;
        
        public EntityWorldSerializer(
            EntityWorld entityWorld, 
            EntityCatalog entityCatalog)
        {
            _entityWorld = entityWorld;

            _componentsCache = new List<ISerializableComponent>();
            _componentSerializer = new ComponentSerializer(entityCatalog, _entityWorld);
        }

        public EntityData[] Serialize()
        {
            IReadOnlyCollection<Entity> activeEntities = _entityWorld.GetAll();
            EntityData[] result = new EntityData[activeEntities.Count];

            int index = 0;
            
            foreach (Entity entity in activeEntities)
            {
                _componentsCache.Clear();
                entity.GetComponents(_componentsCache);
                
                Transform entityTransform = entity.transform;

                Dictionary<string, JToken> componentsMap = new();

                foreach (ISerializableComponent component in _componentsCache)
                {
                    string key = component.GetType().Name;
                    componentsMap[key] = component.Serialize(_componentSerializer);
                }
                
                result[index++] = new EntityData
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Position = entityTransform.position,
                    Rotation = entityTransform.rotation.eulerAngles,
                    Components = componentsMap
                };
            }
            
            _componentsCache.Clear();
            return result;
        }

        public void Deserialize(EntityData[] saveData)
        {
            if (saveData == null)
                return;

            List<(Entity entity, EntityData data)> actualState = UpdateWorldState(saveData);
            
            DeserializeComponentFor(actualState);
        }

        private void DeserializeComponentFor(List<(Entity entity, EntityData data)> pairs)
        {
            foreach ((Entity entity, EntityData data) in pairs)
            {
                if (entity != null && data.Components != null)
                {
                    _componentsCache.Clear();

                    entity.GetComponents(_componentsCache);

                    foreach (ISerializableComponent component in _componentsCache)
                    {
                        string key = component.GetType().Name;
                        
                        if(data.Components.TryGetValue(key, out JToken token))
                            component.Deserialize(_componentSerializer, token);
                    }
                }
            }
            _componentsCache.Clear();
        }
        
        private List<(Entity entity, EntityData data)> UpdateWorldState(EntityData[] saveData)
        {
            IReadOnlyCollection<Entity> currentEntities = _entityWorld.GetAll();

            List<(Entity, EntityData)> result = new();
            
            HashSet<int> idsToRemove = new HashSet<int>();
            
            foreach (Entity entity in currentEntities)
                idsToRemove.Add(entity.Id);

            foreach (EntityData data in saveData)
            {
                Entity entity = null;

                if (_entityWorld.TryGet(data.Id, out Entity existingEntity))
                {
                    if (existingEntity.Name == data.Name)
                    {
                        existingEntity.transform.position = data.Position;
                        existingEntity.transform.rotation = Quaternion.Euler(data.Rotation);
                        entity = existingEntity;
                    }
                    else
                    {
                        _entityWorld.Destroy(data.Id);
                    }

                    idsToRemove.Remove(data.Id);
                }
                
                if (entity == null)
                {
                    entity = _entityWorld.Spawn(
                        data.Name,
                        data.Position,
                        Quaternion.Euler(data.Rotation),
                        data.Id);
                }

                result.Add((entity, data));
            }
            
            foreach (int id in idsToRemove)
                _entityWorld.Destroy(id);

            return result;
        }
    }
}