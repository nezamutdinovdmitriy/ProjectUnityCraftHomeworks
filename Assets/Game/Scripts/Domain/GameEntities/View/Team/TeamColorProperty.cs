using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Game.View.Components
{
    [MaterialProperty("_Color1")]
    public struct TeamColorProperty : IComponentData
    {
        public float4 Value;
    }
}