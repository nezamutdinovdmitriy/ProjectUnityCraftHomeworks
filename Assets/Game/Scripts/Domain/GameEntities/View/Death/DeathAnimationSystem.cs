using Game.Components;
using Rukhanka;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game.View.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct DeathAnimationSystem : ISystem
    {
        private FastAnimatorParameter _deathKey;

        public void OnCreate(ref SystemState state)
        {
            _deathKey = new FastAnimatorParameter("Death");
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new UpdateDeathAnimationJob
            {
                EventsLookup = SystemAPI.GetComponentLookup<DeathEvent>(true),
                Key = _deathKey
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct UpdateDeathAnimationJob : IJobEntity
        {
            [ReadOnly] public FastAnimatorParameter Key;
            [ReadOnly] public ComponentLookup<DeathEvent> EventsLookup;
            
            private void Execute(
                in ModelLink modelLink,
                DynamicBuffer<AnimatorControllerParameterComponent> parameters,
                in AnimatorControllerParameterIndexTableComponent indexTable)
            {
                Entity actor = modelLink.Value;

                if (EventsLookup.HasComponent(actor) == false
                    || EventsLookup.IsComponentEnabled(actor) == false)
                    return;

                var animator = new AnimatorParametersAspect(parameters, indexTable);
                
                animator.SetTrigger(Key);
            }
        }
    }
}