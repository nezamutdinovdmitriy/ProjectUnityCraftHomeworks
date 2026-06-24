using System.Collections.Generic;
using System.Linq;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using SampleGame.Common;

namespace SampleGame.Gameplay
{
    public sealed partial class ComponentSerializer
    {
        #region Countdown
        
        public JToken Serialize(Countdown countdownComponent) 
            => JToken.FromObject(countdownComponent.Current);

        public void Deserialize(Countdown countdown, JToken token) 
            => countdown.Current = token.ToObject<float>();

        #endregion

        #region DestinationPoint
        
        public JToken Serialize(DestinationPoint destinationPointComponent)
        {
            SerializedVector3 serializedPos = destinationPointComponent.Value;
            return JToken.FromObject(serializedPos);
        }

        public void Deserialize(DestinationPoint destinationPointComponent, JToken token)
        {
            SerializedVector3 serializedPos = token.ToObject<SerializedVector3>();
            destinationPointComponent.Value = serializedPos;
        }

        #endregion

        #region Health

        public JToken Serialize(Health healthComponent) 
            => JToken.FromObject(healthComponent.Current);

        public void Deserialize(Health healthComponent, JToken token) 
            => healthComponent.Current = token.ToObject<int>();

        #endregion

        #region ProductionOrder
        
        public JToken Serialize(ProductionOrder productionOrderComponent)
        {
            List<string> configNames = productionOrderComponent.Queue
                .Select(config => config.Name)
                .ToList();
            
            return JToken.FromObject(configNames);
        }

        public void Deserialize(ProductionOrder productionOrderComponent, JToken token)
        {
            List<string> configNames = token.ToObject<List<string>>();
            
            if (configNames == null) return;
            
            List<EntityConfig> loadedQueue = new();
            
            foreach (string name in configNames)
                if (_entityCatalog.FindConfig(name, out EntityConfig config))
                    loadedQueue.Add(config);
            
            productionOrderComponent.Queue = loadedQueue;
        }

        #endregion

        #region ResourceBag
        
        public JToken Serialize(ResourceBag resourceBagComponent)
        {
            ResourceBagData data = new()
            {
                Type = resourceBagComponent.Type,
                Current = resourceBagComponent.Current
            };
            
            return JToken.FromObject(data);
        }

        public void Deserialize(ResourceBag resourceBagComponent, JToken token)
        {
            ResourceBagData dto = token.ToObject<ResourceBagData>();
            
            resourceBagComponent.Type = dto.Type;
            resourceBagComponent.Current = dto.Current;
        }

        #endregion

        #region TargetObject
        
        public JToken Serialize(TargetObject targetObjectComponent)
        {
            int targetId = targetObjectComponent.Value != null
                ? targetObjectComponent.Value.Id : -1;
            
            return JToken.FromObject(targetId);
        }

        public void Deserialize(TargetObject targetObjectComponent, JToken token)
        {
            int targetId = token.ToObject<int>();
            
            if (targetId != -1 && _entityWorld.TryGet(targetId, out Entity targetEntity))
                targetObjectComponent.Value = targetEntity;
            else
                targetObjectComponent.Value = null;
        }

        #endregion
        
        #region Team
        
        public JToken Serialize(Team teamComponent)
            => JToken.FromObject(teamComponent.Type);

        public void Deserialize(Team teamComponent, JToken token) 
            => teamComponent.Type = token.ToObject<TeamType>();

        #endregion
    }
}