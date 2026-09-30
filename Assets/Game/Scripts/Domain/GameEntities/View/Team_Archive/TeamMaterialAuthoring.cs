// using Unity.Entities;
// using UnityEngine;
//
// namespace Game.View.Components
// {
//     public sealed class TeamMaterialAuthoring : MonoBehaviour
//     {
//         [SerializeField] private Material _redTeamMaterial;
//         [SerializeField] private Material _blueTeamMaterial;
//         
//         private sealed class Baker : Baker<TeamMaterialAuthoring>
//         {
//             public override void Bake(TeamMaterialAuthoring authoring)
//             {
//                 DependsOn(authoring._redTeamMaterial);
//                 DependsOn(authoring._blueTeamMaterial);
//                 
//                 Entity entity = GetEntity(TransformUsageFlags.None);
//                 
//                 AddComponent(entity, new TeamMaterials
//                 {
//                     RedTeam = authoring._redTeamMaterial,
//                     BlueTeam = authoring._blueTeamMaterial
//                 });
//             }
//         }
//     }
// }