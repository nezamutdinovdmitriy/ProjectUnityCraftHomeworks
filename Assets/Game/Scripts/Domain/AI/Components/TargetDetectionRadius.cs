using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct TargetDetectionRadius : IComponentData
    {
        public float Value;
    }
}