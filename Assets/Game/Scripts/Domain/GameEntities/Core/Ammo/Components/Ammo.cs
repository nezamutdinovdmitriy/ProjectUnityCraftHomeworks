using System;
using Unity.Entities;

namespace Game.Components
{
    [Serializable]
    public struct Ammo : IComponentData
    {
        public int Value;
    }
}