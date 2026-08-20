using System.Collections.Generic;

namespace Modules.AI
{
    public interface IBehaviourNodeComposite
    {
         IEnumerable<BehaviourNode> Nodes { get; }
    }
}