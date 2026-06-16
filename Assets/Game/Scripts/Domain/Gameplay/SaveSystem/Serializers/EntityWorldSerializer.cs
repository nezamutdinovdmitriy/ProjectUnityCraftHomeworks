using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Entities;
using SampleGame.Common;
using UnityEngine;

namespace Game.Scripts.Domain.Serializers
{
    [Serializable]
    public struct EntitySaveData
    {
        public int Id;
        public string ConfigName;
        public SerializedVector3 Position;
        public SerializedVector3 Rotation;
    }
    
    public class EntityWorldSerializer : ISaveSerializer<EntitySaveData[]>
    {
        private readonly EntityWorld _entityWorld;

        public EntityWorldSerializer(EntityWorld entityWorld) 
            => _entityWorld = entityWorld;

        public EntitySaveData[] Serialize()
        {
            IReadOnlyCollection<Entity> activeEntities = _entityWorld.GetAll();
            
            EntitySaveData[] result = new EntitySaveData[activeEntities.Count];

            int index = 0;

            foreach (Entity entity in activeEntities)
            {
                Transform entityTransform = entity.transform;

                result[index++] = new EntitySaveData()
                {
                    Id = entity.Id,
                    ConfigName = entity.Name,
                    Position = entityTransform.position,
                    Rotation = entityTransform.rotation.eulerAngles
                };
            }

            return result;
        }

        public void Deserialize(EntitySaveData[] data)
        {
            if (data == null)
                return;

            IReadOnlyCollection<Entity> currentEntities = _entityWorld.GetAll();

            HashSet<int> idsToRemove = new HashSet<int>(
                currentEntities.Select(entity => entity.Id));

            foreach (EntitySaveData saveData in data)
            {
                if (_entityWorld.TryGet(saveData.Id, out Entity existingEntity))
                {
                    idsToRemove.Remove(saveData.Id);

                    existingEntity.transform.position = saveData.Position;
                    existingEntity.transform.rotation = saveData.Rotation;
                }
                else
                {
                    _entityWorld.Spawn(
                        saveData.ConfigName,
                        saveData.Position,
                        Quaternion.Euler(saveData.Rotation),
                        saveData.Id);
                }
            }

            foreach (int id in idsToRemove)
                _entityWorld.Destroy(id);
        }
    }
}