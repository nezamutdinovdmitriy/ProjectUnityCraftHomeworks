using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct AttackCooldown : IComponentData
    {
        public float Time;
        public float Duration;
    }
}