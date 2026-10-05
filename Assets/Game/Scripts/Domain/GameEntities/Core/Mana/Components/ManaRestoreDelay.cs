using Unity.Entities;

namespace Game.Components
{
    public struct ManaRestoreDelay : IComponentData, IEnableableComponent
    {
        public float Time;
        public float Delay;
    }
}