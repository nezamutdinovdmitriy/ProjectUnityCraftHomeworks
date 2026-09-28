using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct ArrowReplenishmentCooldown : IComponentData
    {
        public float Time;
        public float Duration;
    }
}