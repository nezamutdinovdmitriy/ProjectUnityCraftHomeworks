using Newtonsoft.Json.Linq;

namespace SampleGame.Gameplay
{
    public interface IComponentSerializer
    {
        public JToken Serialize(Countdown countdownComponent);
        public void Deserialize(Countdown countdown, JToken token);
        
        public JToken Serialize(DestinationPoint destinationPointComponent);
        public void Deserialize(DestinationPoint destinationPointComponent, JToken token);
        
        public JToken Serialize(Health healthComponent);
        public void Deserialize(Health healthComponent, JToken token);
        
        public JToken Serialize(ProductionOrder productionOrderComponent);
        public void Deserialize(ProductionOrder productionOrderComponent, JToken token);
        
        public JToken Serialize(ResourceBag resourceBagComponent);
        public void Deserialize(ResourceBag resourceBagComponent, JToken token);
        
        public JToken Serialize(TargetObject targetObjectComponent);
        public void Deserialize(TargetObject targetObjectComponent, JToken token);
        
        public JToken Serialize(Team teamComponent);
        public void Deserialize(Team teamComponent, JToken token);
    }
}