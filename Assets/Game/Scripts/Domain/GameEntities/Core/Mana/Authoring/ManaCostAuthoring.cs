using Unity.Entities;
using UnityEngine;

namespace Game.Components.Authoring
{
    public sealed class ManaCostAuthoring : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float _manaCost = 5f;
        
        private sealed class Baker : Baker<ManaCostAuthoring>
        {
            public override void Bake(ManaCostAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new ManaCost
                {
                    Value = authoring._manaCost
                });
            }
        }
    }
}