using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct DetectRadius : IComponentData
    {
        public float Value;
    }
}