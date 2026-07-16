using System;
using System.Collections.Generic;

namespace Atomic.Entities
{
    public class TestSceneEntityScope : IDisposable
    {
        private readonly List<SceneEntity> _entities = new();
        
        public SceneEntity NewEntity(in SceneEntity.CreateArgs args = default)
        {
            SceneEntity entity = SceneEntity.Create(in args);
            _entities.Add(entity);
            return entity;
        }

        public void Dispose()
        {
            for (int i = 0, count = _entities.Count; i < count; i++) 
                _entities[i].Dispose();

            _entities.Clear();
        }
    }
}