using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct MaxMana : IComponentData
    {
        public float Value;
    }
}