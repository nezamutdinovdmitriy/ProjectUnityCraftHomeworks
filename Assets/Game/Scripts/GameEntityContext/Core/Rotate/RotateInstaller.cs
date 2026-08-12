using System;
using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    [Serializable]
    public class RotateInstaller : IEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private float _rotateSpeed;
        
        public void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.RotateSpeed, new Const<float>(_rotateSpeed));
            
            entity.AddValue(GameEntityAPI.RotateRequest, new Request<Vector3>());
            entity.AddValue(GameEntityAPI.RotateCommand, new Command<RotateArgs>());
            
            entity.AddBehaviour(new RotateBehaviour());
        }
    }
}