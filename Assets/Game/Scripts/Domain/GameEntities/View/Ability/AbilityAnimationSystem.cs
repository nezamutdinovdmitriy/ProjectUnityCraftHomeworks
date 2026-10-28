using Game.Components;
using Rukhanka;
using Unity.Entities;

namespace Game.View.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct AbilityAnimationSystem : ISystem
    {
        private FastAnimatorParameter _attackKey;
        private ComponentLookup<AbilityStartedEvent> _attackStartedEventLookup;
        private AnimatorParametersAspect _animator;

        public void OnCreate(ref SystemState state)
        {
            _attackKey = new FastAnimatorParameter("Fire");
            _attackStartedEventLookup = SystemAPI.GetComponentLookup<AbilityStartedEvent>(true);
            _animator = new AnimatorParametersAspect();
        }

        public void OnUpdate(ref SystemState state)
        {
            _attackStartedEventLookup.Update(ref state);

            foreach (var (
                         modelLink,
                         indexTable,
                         parameters)
                     in SystemAPI.Query<
                         RefRO<ModelLink>,
                         RefRO<AnimatorControllerParameterIndexTableComponent>,
                         DynamicBuffer<AnimatorControllerParameterComponent>>())
            {
                Entity model = modelLink.ValueRO.Value;
                
                if (_attackStartedEventLookup.HasComponent(model) == false
                    || _attackStartedEventLookup.IsComponentEnabled(model) == false)
                    continue;
                
                _animator.parametersArr = parameters;
                _animator.indexTable = indexTable.ValueRO;
                
                _animator.SetTrigger(_attackKey);
            }
        }
    }
}