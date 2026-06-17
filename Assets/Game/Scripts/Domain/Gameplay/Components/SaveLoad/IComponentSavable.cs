namespace SampleGame.Gameplay
{
    public interface IComponentSavable
    {
        public void Accept(IComponentVisitor visitor);
    }
}