using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct AttackCooldown : IComponentData, IEnableableComponent
    {
        public float Time;
        public float Duration;
    }
}