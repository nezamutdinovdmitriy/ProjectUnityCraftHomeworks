using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntities
{
    public class BulletViewInstaller : SceneEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private TrailRenderer _bulletTrail;
        
        public override void Install(IGameEntity entity)
        {
            entity.WhenEnable(() =>
            {
                _bulletTrail.emitting = true;
                _bulletTrail.Clear();
            });
            
            entity.WhenDisable(() =>
            {
                _bulletTrail.Clear();
                _bulletTrail.emitting = false;
            });
        }
    }
}