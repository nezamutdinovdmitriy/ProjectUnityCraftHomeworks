using Game.Components;
using Rukhanka;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Game.View.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct MovementAnimationSystem : ISystem
    {
        private FastAnimatorParameter _isMovingKey;

        public void OnCreate(ref SystemState state)
        {
            _isMovingKey = new FastAnimatorParameter("IsMoving");
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new UpdateMovementAnimationJob
            {
                EventsLookup = SystemAPI.GetComponentLookup<MovementEvent>(true),
                Key = _isMovingKey
            }.ScheduleParallel(state.Dependency);
        }
        
        [BurstCompile]
        private partial struct UpdateMovementAnimationJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<MovementEvent> EventsLookup;
            [ReadOnly] public FastAnimatorParameter Key;
            
            private void Execute(
                in ModelLink modelLink,
                DynamicBuffer<AnimatorControllerParameterComponent> parameters,
                in AnimatorControllerParameterIndexTableComponent indexTable)
            {
                Entity actor = modelLink.Value;

                if (EventsLookup.HasComponent(actor) == false)
                    return;

                bool isMoving = EventsLookup.IsComponentEnabled(actor);

                var animator = new AnimatorParametersAspect(parameters, indexTable);
                
                animator.SetBoolParameter(Key, isMoving);
            }
        }
    }
}