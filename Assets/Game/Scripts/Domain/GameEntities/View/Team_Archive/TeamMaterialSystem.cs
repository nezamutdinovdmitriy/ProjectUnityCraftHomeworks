// using System;
// using Game.Components;
// using Game.View.Components;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Rendering;
// using UnityEngine;
// using UnityEngine.Rendering;
//
// namespace Game.View.Systems
// {
//     [UpdateInGroup(typeof(UpdatePresentationSystemGroup))]
//     public partial struct TeamMaterialSystem : ISystem
//     {
//         private BatchMaterialID _redMaterialID;
//         private BatchMaterialID _blueMaterialID;
//         private bool _materialsRegistered;
//
//         private ComponentLookup<Team> _teamLookup;
//         private ComponentLookup<MaterialMeshInfo> _materialLookup;
//
//         private EntityQuery _pendingSetupQuery;
//
//         public void OnCreate(ref SystemState state)
//         {
//             state.RequireForUpdate<TeamMaterials>();
//             state.RequireForUpdate<TeamMaterialPart>();
//
//             _teamLookup = state.GetComponentLookup<Team>(true);
//             _materialLookup = state.GetComponentLookup<MaterialMeshInfo>();
//
//             _pendingSetupQuery = SystemAPI.QueryBuilder()
//                 .WithAll<TeamMaterialPart, TeamMaterialPendingSetup>()
//                 .Build();
//         }
//
//         public void OnUpdate(ref SystemState state)
//         {
//             if (_materialsRegistered == false)
//                 if (TryRegisterMaterials(ref state) == false)
//                     return;
//
//             PreparePendingParts(ref state);
//
//             _teamLookup.Update(ref state);
//             _materialLookup.Update(ref state);
//
//             state.Dependency = new ApplyTeamMaterialsJob
//             {
//                 TeamLookup = _teamLookup,
//                 MaterialLookup = _materialLookup,
//                 RedMaterialID = _redMaterialID,
//                 BlueMaterialID = _blueMaterialID
//             }.Schedule(state.Dependency);
//         }
//
//         public void OnDestroy(ref SystemState state) => state.Dependency.Complete();
//
//         private bool TryRegisterMaterials(ref SystemState state)
//         {
//             EntitiesGraphicsSystem graphicsSystem = state.World
//                 .GetExistingSystemManaged<EntitiesGraphicsSystem>();
//
//             if (graphicsSystem == null)
//                 return false;
//
//             state.EntityManager.CompleteDependencyBeforeRO<TeamMaterials>();
//
//             TeamMaterials materials = SystemAPI.GetSingleton<TeamMaterials>();
//
//             Material redMaterial = materials.RedTeam.Value;
//             Material blueMaterial = materials.BlueTeam.Value;
//
//             if (redMaterial == null || blueMaterial == null)
//                 return false;
//
//             _redMaterialID = graphicsSystem.RegisterMaterial(redMaterial);
//             _blueMaterialID = graphicsSystem.RegisterMaterial(blueMaterial);
//
//             _materialsRegistered = true;
//             return true;
//         }
//
//         private void PreparePendingParts(ref SystemState state)
//         {
//             if (_pendingSetupQuery.IsEmpty)
//                 return;
//
//             state.Dependency.Complete();
//
//             EntityManager entityManager = state.EntityManager;
//
//             entityManager.CompleteDependencyBeforeRW<MaterialMeshInfo>();
//
//             foreach ((DynamicBuffer<TeamMaterialPart> parts,
//                          EnabledRefRW<TeamMaterialPendingSetup> pendingSetup)
//                      in SystemAPI.Query<
//                          DynamicBuffer<TeamMaterialPart>,
//                          EnabledRefRW<TeamMaterialPendingSetup>>())
//             {
//                 bool allPartsPrepared = true;
//
//                 for (int i = 0; i < parts.Length; i++)
//                 {
//                     Entity partEntity = parts[i].Value;
//
//                     if (entityManager.HasComponent<MaterialMeshInfo>(partEntity) == false)
//                     {
//                         allPartsPrepared = false;
//                         continue;
//                     }
//
//                     MaterialMeshInfo materialInfo = entityManager
//                         .GetComponentData<MaterialMeshInfo>(partEntity);
//
//                     if (materialInfo.HasMaterialMeshIndexRange == false)
//                         continue;
//
//                     MaterialMeshInfo preparedInfo = ResolveSingleEntry(entityManager, partEntity, materialInfo);
//                    
//                     entityManager.SetComponentData(partEntity, preparedInfo);
//                 }
//
//                 if (allPartsPrepared)
//                     pendingSetup.ValueRW = false;
//             }
//         }
//
//         private static MaterialMeshInfo ResolveSingleEntry(
//             EntityManager entityManager,
//             Entity partEntity,
//             MaterialMeshInfo materialInfo)
//         {
//             var range = materialInfo.MaterialMeshIndexRange;
//
//             if (range.length == 1)
//             {
//                 RenderMeshArray renderMeshArray = entityManager
//                     .GetSharedComponentManaged<RenderMeshArray>(partEntity);
//
//                 MaterialMeshIndex entry = renderMeshArray.MaterialMeshIndices[range.start];
//
//                 return MaterialMeshInfo.FromRenderMeshArrayIndices(
//                     entry.MaterialIndex,
//                     entry.MeshIndex,
//                     checked((ushort) entry.SubMeshIndex));
//             }
//
//             throw new InvalidOperationException(
//                 $"Team material part {partEntity} has " +
//                 $"{range.length} material/mesh entries. " +
//                 "Expected exactly one.");
//         }
//
//         [BurstCompile]
//         [WithDisabled(typeof(TeamMaterialPendingSetup))]
//         private partial struct ApplyTeamMaterialsJob : IJobEntity
//         {
//             [ReadOnly] public ComponentLookup<Team> TeamLookup;
//             [ReadOnly] public BatchMaterialID RedMaterialID;
//             [ReadOnly] public BatchMaterialID BlueMaterialID;
//
//             public ComponentLookup<MaterialMeshInfo> MaterialLookup;
//
//             private void Execute(in ModelLink modelLink, in DynamicBuffer<TeamMaterialPart> parts)
//             {
//                 if (TeamLookup.TryGetComponent(modelLink.Value, out Team team) == false)
//                     return;
//
//                 BatchMaterialID targetMaterialID = team.Value == TeamType.Red
//                     ? RedMaterialID
//                     : BlueMaterialID;
//
//                 for (int i = 0; i < parts.Length; i++)
//                 {
//                     Entity partEntity = parts[i].Value;
//
//                     if (MaterialLookup.TryGetComponent(partEntity, out MaterialMeshInfo materialInfo) == false)
//                         continue;
//
//                     MaterialMeshInfo updatedInfo = materialInfo;
//                     updatedInfo.MaterialID = targetMaterialID;
//
//                     if (updatedInfo.Material == materialInfo.Material)
//                         continue;
//
//                     MaterialLookup[partEntity] = updatedInfo;
//                 }
//             }
//         }
//     }
// }