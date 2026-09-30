using Game.Components;
using Rukhanka;
using Unity.Entities;

namespace Game.View.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct TakeDamageAnimationSystem : ISystem
    {
        private FastAnimatorParameter _takeDamageKey;
        private BufferLookup<TakeDamageEvent> _takeDamageEventLookup;
        
        public void OnCreate(ref SystemState state)
        {
            _takeDamageKey = new FastAnimatorParameter("TakeDamage");
            _takeDamageEventLookup = SystemAPI.GetBufferLookup<TakeDamageEvent>(isReadOnly: true);
        }

        public void OnUpdate(ref SystemState state)
        {
            _takeDamageEventLookup.Update(ref state);
            
            foreach ((RefRO<ModelLink> modelLink,
                         DynamicBuffer<AnimatorControllerParameterComponent> parameters,
                         RefRO<AnimatorControllerParameterIndexTableComponent> indexTable)
                     in SystemAPI.Query<
                         RefRO<ModelLink>,
                         DynamicBuffer<AnimatorControllerParameterComponent>,
                         RefRO<AnimatorControllerParameterIndexTableComponent>>())
            {
                Entity model = modelLink.ValueRO.Value;
                if(_takeDamageEventLookup.TryGetBuffer(model, out DynamicBuffer<TakeDamageEvent> events) == false
                   || events.IsEmpty)
                    continue;

                AnimatorParametersAspect animator =
                    new AnimatorParametersAspect(parameters, indexTable.ValueRO);

                animator.SetTrigger(_takeDamageKey);
            }
        }
    }
}