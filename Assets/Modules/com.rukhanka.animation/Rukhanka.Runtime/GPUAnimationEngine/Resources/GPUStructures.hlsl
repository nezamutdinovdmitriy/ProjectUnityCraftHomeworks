
#ifndef GPU_STRUCTURES_HLSL_
#define GPU_STRUCTURES_HLSL_

/////////////////////////////////////////////////////////////////////////////////

//  struct RigDefinition
ByteAddressBuffer rigDefinitions;
//  struct RigBone
ByteAddressBuffer rigBones;
//  struct AnimationClip
ByteAddressBuffer animationClips;
//  struct HumanRotationData
ByteAddressBuffer humanRotationDataBuffer;
int animatedBonesCount;

/////////////////////////////////////////////////////////////////////////////////

#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/Debug.hlsl"
#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/GPUStructures/BoneTransform.hlsl"
#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/GPUStructures/AnimationClip.hlsl"
#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/GPUStructures/Track.hlsl"
#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/GPUStructures/SkinnedMeshBone.hlsl"
#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/GPUStructures/RigDefinition.hlsl"
#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/GPUStructures/RigBone.hlsl"
#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/GPUStructures/HumanRotationData.hlsl"
#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/GPUStructures/AnimationToProcess.hlsl"
#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/GPUStructures/AvatarMask.hlsl"
#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/GPUStructures/PerfectHashTable.hlsl"
#include "Assets/Modules/com.rukhanka.animation/Rukhanka.Runtime/Common/Shaders/GPUStructures/SkinMatrix.hlsl"

/////////////////////////////////////////////////////////////////////////////////

struct AnimationJob
{
    int rigDefinitionIndex;
    int animatedBoneIndexOffset;
    int2 animationsToProcessRange;
};

/////////////////////////////////////////////////////////////////////////////////

struct SkinnedMeshWorkload
{
    int skinMatrixBaseOutIndex;
    int boneRemapTableIndex;
    int skinMatricesCount;
    int rootBoneIndex;
    int animatedBoneIndexOffset;
    float4x4 skinnedRootBoneToEntityTransform;
};

/////////////////////////////////////////////////////////////////////////////////

struct AnimatedBoneWorkload
{
    int boneIndexInRig;
    int animationJobIndex;
};

/////////////////////////////////////////////////////////////////////////////////

StructuredBuffer<AnimatedBoneWorkload> animatedBoneWorkload;
StructuredBuffer<AnimationJob> animationJobs;
StructuredBuffer<AnimationToProcess> animationsToProcess;

#endif
