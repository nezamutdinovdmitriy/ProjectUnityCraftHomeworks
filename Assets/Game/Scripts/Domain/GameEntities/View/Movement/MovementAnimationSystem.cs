using Game.Components;
using Rukhanka;
using Unity.Burst;
using Unity.Entities;

namespace Game.View.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct MovementAnimationSystem : ISystem
    {
        private FastAnimatorParameter _isMovingKey;
        private ComponentLookup<MovementEvent> _movementEventLookup;
        private AnimatorParametersAspect _animator;

        public void OnCreate(ref SystemState state)
        {
            _isMovingKey = new FastAnimatorParameter("IsMoving");
            _movementEventLookup = SystemAPI.GetComponentLookup<MovementEvent>();
            _animator = new AnimatorParametersAspect();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _movementEventLookup.Update(ref state);

            foreach (var (
                         modelLinkRO,
                         parameters,
                         indexTable)
                     in SystemAPI.Query<
                         RefRO<ModelLink>,
                         DynamicBuffer<AnimatorControllerParameterComponent>,
                         RefRO<AnimatorControllerParameterIndexTableComponent>>())
            {
                Entity model = modelLinkRO.ValueRO.Value;

                if(_movementEventLookup.HasComponent(model) == false)
                    continue;
                
                bool isMoving = _movementEventLookup.IsComponentEnabled(model);

                _animator.parametersArr = parameters;
                _animator.indexTable = indexTable.ValueRO;
                
                _animator.SetBoolParameter(_isMovingKey, isMoving);
            }
        }
    }
}