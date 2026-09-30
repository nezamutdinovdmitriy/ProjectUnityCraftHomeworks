using Unity.Entities;
using Unity.Mathematics;

namespace Game.View.Components
{
    public struct TeamColors : IComponentData
    {
        public float4 Red;
        public float4 Blue;
    }
}