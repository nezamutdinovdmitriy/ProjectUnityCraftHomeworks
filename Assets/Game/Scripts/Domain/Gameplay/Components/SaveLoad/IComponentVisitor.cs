namespace SampleGame.Gameplay
{
    public interface IComponentVisitor
    {
        public void Visit(Countdown countdownComponent);
        public void Visit(DestinationPoint destinationPointComponent);
        public void Visit(Health healthComponent);
        public void Visit(ProductionOrder productionOrderComponent);
        public void Visit(ResourceBag resourceBagComponent);
        public void Visit(TargetObject targetObjectComponent);
        public void Visit(Team teamComponent);
    }
}