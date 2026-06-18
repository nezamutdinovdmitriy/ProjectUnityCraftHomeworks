using System.Collections.Generic;
using System.Linq;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using SampleGame.Common;

namespace SampleGame.Gameplay
{
    public sealed partial class JsonComponentVisitor
    {
        public void Visit(Countdown countdownComponent)
        {
            if (_mode == VisitorMode.Save)
                SaveData = JToken.FromObject(countdownComponent.Current);
            else if (_mode == VisitorMode.Load && SaveData != null)
                countdownComponent.Current = SaveData.ToObject<float>();
        }

        public void Visit(Health healthComponent)
        {
            if (_mode == VisitorMode.Save)
                SaveData = JToken.FromObject(healthComponent.Current);
            else if (_mode == VisitorMode.Load && SaveData != null)
                healthComponent.Current = SaveData.ToObject<int>();
        }

        public void Visit(DestinationPoint destinationPointComponent)
        {
            if (_mode == VisitorMode.Save)
            {
                SerializedVector3 serializedPos = destinationPointComponent.Value;
                SaveData = JToken.FromObject(serializedPos);
            }
            else if (_mode == VisitorMode.Load && SaveData != null)
            {
                SerializedVector3 serializedPos = SaveData.ToObject<SerializedVector3>();
                destinationPointComponent.Value = serializedPos;
            }
        }

        public void Visit(ProductionOrder productionOrderComponent)
        {
            if (_mode == VisitorMode.Save)
            {
                List<string> configNames = productionOrderComponent.Queue
                    .Select(config => config.Name)
                    .ToList();

                SaveData = JToken.FromObject(configNames);
            }
            else if (_mode == VisitorMode.Load && SaveData != null)
            {
                List<string> configNames = SaveData.ToObject<List<string>>();
                if (configNames == null) return;

                List<EntityConfig> loadedQueue = new();

                foreach (string name in configNames)
                    if (_entityCatalog.FindConfig(name, out EntityConfig config))
                        loadedQueue.Add(config);

                productionOrderComponent.Queue = loadedQueue;
            }
        }

        public void Visit(ResourceBag resourceBagComponent)
        {
            if (_mode == VisitorMode.Save)
            {
                ResourceBagData data = new()
                {
                    Type = resourceBagComponent.Type,
                    Current = resourceBagComponent.Current
                };

                SaveData = JToken.FromObject(data);
            }
            else if (_mode == VisitorMode.Load && SaveData != null)
            {
                ResourceBagData dto = SaveData.ToObject<ResourceBagData>();

                resourceBagComponent.Type = dto.Type;
                resourceBagComponent.Current = dto.Current;
            }
        }

        public void Visit(TargetObject targetObjectComponent)
        {
            if (_mode == VisitorMode.Save)
            {
                int targetId = targetObjectComponent.Value != null
                    ? targetObjectComponent.Value.Id
                    : -1;

                SaveData = JToken.FromObject(targetId);
            }
            else if (_mode == VisitorMode.Load && SaveData != null)
            {
                int targetId = SaveData.ToObject<int>();

                if (targetId != -1 && _entityWorld.TryGet(targetId, out Entity targetEntity))
                    targetObjectComponent.Value = targetEntity;
                else
                    targetObjectComponent.Value = null;
            }
        }

        public void Visit(Team teamComponent)
        {
            if (_mode == VisitorMode.Save)
                SaveData = JToken.FromObject(teamComponent.Type);
            else if (_mode == VisitorMode.Load && SaveData != null)
                teamComponent.Type = SaveData.ToObject<TeamType>();
        }
    }
}