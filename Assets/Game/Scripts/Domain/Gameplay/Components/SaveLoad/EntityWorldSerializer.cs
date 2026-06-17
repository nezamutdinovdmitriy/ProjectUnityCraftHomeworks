using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly EntityWorld _entityWorld;
        private readonly EntityCatalog _entityCatalog;
        
        private readonly List<IComponentSavable> _componentsCache = new();

        public EntityWorldSerializer(
            EntityWorld entityWorld, 
            EntityCatalog entityCatalog)
        {
            _entityWorld = entityWorld;
            _entityCatalog = entityCatalog;
        }

        public EntityData[] Serialize()
        {
            IReadOnlyCollection<Entity> activeEntities = _entityWorld.GetAll();
            EntityData[] result = new EntityData[activeEntities.Count];

            int index = 0;

            foreach (Entity entity in activeEntities)
            {
                Transform entityTransform = entity.transform;

                _componentsCache.Clear();
                entity.GetComponents(_componentsCache);

                Dictionary<string, JToken> componentsMap = new();

                foreach (IComponentSavable component in _componentsCache)
                {
                    JsonComponentVisitor saveVisitor = new();
                    
                    component.Accept(saveVisitor);

                    string key = component.GetType().Name;
                    componentsMap[key] = saveVisitor.SaveData;
                }
                
                result[index++] = new EntityData()
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

            IReadOnlyCollection<Entity> currentEntities = _entityWorld.GetAll();

            HashSet<int> idsToRemove = new HashSet<int>(
                currentEntities.Select(entity => entity.Id));

            foreach (EntityData data in saveData)
            {
                Entity entity;
                
                if (_entityWorld.TryGet(data.Id, out Entity existingEntity))
                {
                    idsToRemove.Remove(data.Id);

                    existingEntity.transform.position = data.Position;
                    existingEntity.transform.rotation = Quaternion.Euler(data.Rotation);
                    entity = existingEntity;
                }
                else
                {
                   entity = _entityWorld.Spawn(
                        data.Name,
                        data.Position,
                        Quaternion.Euler(data.Rotation),
                        data.Id);
                }

                if (entity != null
                    && data.Components != null)
                {
                    _componentsCache.Clear();
                    entity.GetComponents(_componentsCache);

                    foreach (IComponentSavable component in _componentsCache)
                    {
                        string key = component.GetType().Name;

                        if (data.Components.TryGetValue(key, out JToken componentData))
                        {
                            JsonComponentVisitor loadVisitor = new(
                                componentData, 
                                _entityCatalog, 
                                _entityWorld);
                            
                            component.Accept(loadVisitor);
                        }
                    }
                }
            }

            foreach (int id in idsToRemove)
                _entityWorld.Destroy(id);
            
            _componentsCache.Clear();
        }
    }
}