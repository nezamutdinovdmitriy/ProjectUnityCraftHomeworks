using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct HealAmount : IComponentData
    {
        public float Value;
    }
}