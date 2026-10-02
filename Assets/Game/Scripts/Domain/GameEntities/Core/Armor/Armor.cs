using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct Armor : IComponentData
    {
        public float Value;
    }
}