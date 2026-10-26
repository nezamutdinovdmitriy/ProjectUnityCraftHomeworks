using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Game.Components.Authoring
{
    [DisallowMultipleComponent]
    public sealed class UnitSpawnPointsAuthoring : MonoBehaviour
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private uint _seed = 1;
        
        private sealed class Baker : Baker<UnitSpawnPointsAuthoring>
        {
            public override void Bake(UnitSpawnPointsAuthoring authoring)
            {
                if (authoring._points.Length == 0)
                    Debug.Log("UnitSpawnPoints: assign at least one point!");
                
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                    
                AddComponent(entity, new UnitSpawnRandom
                {
                    Generator = new Random(authoring._seed)
                });

                DynamicBuffer<UnitSpawnPoint> points = AddBuffer<UnitSpawnPoint>(entity);

                foreach (Transform point in authoring._points)
                {
                    points.Add(new UnitSpawnPoint
                    {
                        Value = GetEntity(point, TransformUsageFlags.Dynamic)
                    });
                }
            }
        }
    }
}