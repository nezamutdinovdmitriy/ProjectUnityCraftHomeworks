using Unity.Entities;

namespace Game.Components
{
    public partial struct AttackHitDelay : IComponentData, IEnableableComponent
    {
        public float Time;
        public float Delay;
    } 
}