using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class TeamAuthoring : MonoBehaviour
    {
        [SerializeField] private TeamType _team;

        private sealed class Baker : Baker<TeamAuthoring>
        {
            public override void Bake(TeamAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);

                AddComponent(entity, new Team {Value = authoring._team});
            }
        }
    }
}