using Unity.Entities;
using Unity.Mathematics;

namespace Game.Components
{
    public struct MovementRequest : IComponentData, IEnableableComponent
    {
        public float3 Direction;
    }
}