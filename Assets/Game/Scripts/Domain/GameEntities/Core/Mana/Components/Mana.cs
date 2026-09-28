using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct Mana : IComponentData
    {
        public float Value;
    }
}