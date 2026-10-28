using Game.Components;
using Rukhanka;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game.View.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct AbilityAnimationSystem : ISystem
    {
        private FastAnimatorParameter _attackKey;

        public void OnCreate(ref SystemState state)
        {
            _attackKey = new FastAnimatorParameter("Fire");
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new UpdateAbilityAnimationJob
            {
                EventsLookup = SystemAPI.GetComponentLookup<AbilityStartedEvent>(true),
                Key = _attackKey
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct UpdateAbilityAnimationJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<AbilityStartedEvent> EventsLookup;
            [ReadOnly] public FastAnimatorParameter Key;
            
            private void Execute(
                in ModelLink modelLink,
                in AnimatorControllerParameterIndexTableComponent indexTable,
                DynamicBuffer<AnimatorControllerParameterComponent> parameters)
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