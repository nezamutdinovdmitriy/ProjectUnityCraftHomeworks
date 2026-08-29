using Modules.AI;

namespace SampleGame.AI
{
    public class DefaultCommandNode : BehaviourNode
    {
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            return BehaviourResult.Failure;
        }
    }
}