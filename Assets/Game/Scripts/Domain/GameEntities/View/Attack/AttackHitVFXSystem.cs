using Game.Components;
using Unity.Entities;
using UnityEngine;
using static Unity.Entities.SystemAPI.ManagedAPI;

namespace Game.View.Systems
{
    // [UpdateInGroup(typeof(PresentationSystemGroup))]
    // public partial struct AttackHitVFXSystem : ISystem
    // {
    //     private ComponentLookup<AttackHitEvent> _takeDamageEventLookup;
    //
    //     public void OnCreate(ref SystemState state)
    //     {
    //         _takeDamageEventLookup = SystemAPI.GetComponentLookup<AttackHitEvent>();
    //     }
    //
    //     public void OnUpdate(ref SystemState state)
    //     {
    //         _takeDamageEventLookup.Update(ref state);
    //
    //         foreach (var (
    //                      particleSystem,
    //                      modelLink)
    //                  in SystemAPI.Query<
    //                      UnityEngineComponent<ParticleSystem>,
    //                      RefRO<ModelLink>>())
    //         {
    //             Entity model = modelLink.ValueRO.Value;
    //             
    //             if(_takeDamageEventLookup.HasComponent(model) == false
    //                 || _takeDamageEventLookup.IsComponentEnabled(model) == false)
    //                 continue;
    //
    //             particleSystem.Value.Play(withChildren: true);
    //         }
    //     }
    // }
}