using Game.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Game.View.Components
{
    [UpdateInGroup(typeof(UpdatePresentationSystemGroup))]
    public partial struct TeamColorSystem : ISystem
    {
        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<TeamColorProperty> _teamColorLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TeamColors>();
            state.RequireForUpdate<TeamColorProperty>();

            _teamLookup = SystemAPI.GetComponentLookup<Team>(true);
            _teamColorLookup = SystemAPI.GetComponentLookup<TeamColorProperty>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _teamColorLookup.Update(ref state);
            _teamLookup.Update(ref state);

            TeamColors colors = SystemAPI.GetSingleton<TeamColors>();
            
            state.Dependency = new ApplyTeamColorsJob
            {
                TeamLookup = _teamLookup,
                TeamColors = colors,
                ColorProperty = _teamColorLookup
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        private partial struct ApplyTeamColorsJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<Team> TeamLookup;
            [ReadOnly] public TeamColors TeamColors;

            public ComponentLookup<TeamColorProperty> ColorProperty;

            private void Execute(in ModelLink modelLink, in DynamicBuffer<TeamMaterialPart> parts)
            {
                if (TeamLookup.TryGetComponent(modelLink.Value, out Team team) == false)
                    return;
                
                float4 targetColor = team.Value == TeamType.Red
                    ? TeamColors.Red
                    : TeamColors.Blue;

                for (int i = 0; i < parts.Length; i++)
                {
                    Entity partEntity = parts[i].Value;
                    
                    if (ColorProperty.TryGetComponent(partEntity, out TeamColorProperty color) == false)
                        continue;

                    if (math.all(color.Value == targetColor))
                        continue;
                    
                    color.Value = targetColor;
                    ColorProperty[partEntity] = color;
                }
            }
        }
    }
}