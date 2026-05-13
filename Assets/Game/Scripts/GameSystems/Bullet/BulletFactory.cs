using Game.Spawn;
using UnityEngine;

namespace Game
{
    public class BulletFactory : Factory<Bullet>
    {
        [SerializeField]
        private BulletConfig _config;

        public override Bullet Create()
        {
            Bullet instance = base.Create();
            
            instance.Construct(_config);

            return instance;
        }
    }
}