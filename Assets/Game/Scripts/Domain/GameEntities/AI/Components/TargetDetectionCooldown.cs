using Unity.Entities;

namespace Game.Components
{
    public struct TargetDetectionCooldown : IComponentData, IEnableableComponent
    {
        public float Time;
        public float Interval;
    }
}