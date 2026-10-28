using Game.Components;
using Rukhanka;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game.View.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct TakeDamageAnimationSystem : ISystem
    {
        private FastAnimatorParameter _takeDamageKey;
        
        public void OnCreate(ref SystemState state)
        {
            _takeDamageKey = new FastAnimatorParameter("TakeDamage");
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new UpdateTakeDamageAnimationJob
            {
                Key = _takeDamageKey,
                EventsLookup = SystemAPI.GetBufferLookup<TakeDamageEvent>(true)
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct UpdateTakeDamageAnimationJob : IJobEntity
        {
            [ReadOnly] public FastAnimatorParameter Key;
            [ReadOnly] public BufferLookup<TakeDamageEvent> EventsLookup;
            
            private void Execute(
                in ModelLink modelLink,
                DynamicBuffer<AnimatorControllerParameterComponent> parameters,
                in AnimatorControllerParameterIndexTableComponent indexTable)
            {
                Entity actor = modelLink.Value;

                if (EventsLookup.TryGetBuffer(
                        actor,
                        out DynamicBuffer<TakeDamageEvent> events) == false
                    || events.IsEmpty)
                    return;

                var animator = new AnimatorParametersAspect(parameters, indexTable);
                
                animator.SetTrigger(Key);
            }
        }
    }
}