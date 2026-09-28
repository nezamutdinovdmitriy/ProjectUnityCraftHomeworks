using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct Lifetime : IComponentData
    {
        public float Value;
    }
}