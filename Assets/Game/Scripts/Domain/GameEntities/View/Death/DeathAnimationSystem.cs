using Game.Components;
using Rukhanka;
using Unity.Entities;

namespace Game.View.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct DeathAnimationSystem : ISystem
    {
        private FastAnimatorParameter _deathKey;
        private ComponentLookup<DeathEvent> _deathEventLookup;
        private AnimatorParametersAspect _animator;

        public void OnCreate(ref SystemState state)
        {
            _deathKey = new FastAnimatorParameter("Death");
            _deathEventLookup = SystemAPI.GetComponentLookup<DeathEvent>(isReadOnly: true);
            
            _animator = new AnimatorParametersAspect();
        }

        public void OnUpdate(ref SystemState state)
        {
            _deathEventLookup.Update(ref state);

            foreach ((RefRO<ModelLink> modelLink,
                         DynamicBuffer<AnimatorControllerParameterComponent> parameters,
                         RefRO<AnimatorControllerParameterIndexTableComponent> indexTable)
                     in SystemAPI.Query<
                         RefRO<ModelLink>,
                         DynamicBuffer<AnimatorControllerParameterComponent>,
                         RefRO<AnimatorControllerParameterIndexTableComponent>>())
            {
                Entity model = modelLink.ValueRO.Value;

                if (_deathEventLookup.IsComponentEnabled(model) == false)
                    continue;

                _animator.parametersArr = parameters;
                _animator.indexTable = indexTable.ValueRO;
                
                _animator.SetTrigger(_deathKey);
            }
        }
    }
}