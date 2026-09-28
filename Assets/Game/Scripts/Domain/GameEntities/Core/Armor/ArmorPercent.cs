using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct ArmorPercent : IComponentData
    {
        public float Value;
    }
}