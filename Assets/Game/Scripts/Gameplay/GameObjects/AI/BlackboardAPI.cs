using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    [BlackboardAPI]
    public static class BlackboardAPI
    {
        public static readonly BlackboardValueKey<GameObject> Character = new(nameof(Character));
    }
}