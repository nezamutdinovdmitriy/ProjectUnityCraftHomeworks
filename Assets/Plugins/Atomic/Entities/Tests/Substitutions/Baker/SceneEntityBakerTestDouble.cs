using System;
using UnityEngine;
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Atomic.Entities
{
    [AddComponentMenu("")]
    public class SceneEntityBakerTestDouble : SceneEntityBaker<IEntity>
    {
        public static int CreateCallCount;

        public Func<IEntity> CreateMethod = () => new Entity();

        protected override IEntity Create()
        {
            CreateCallCount++;
            return CreateMethod.Invoke(); // простая пустая сущность
        }
    }
}