using UnityEngine;

namespace Game
{
    public static class BulletLayerHelper
    {
        private const string PlayerBulletKey = "PlayerBullet";
        private const string EnemyBulletKey = "EnemyBullet";
        private const string DefaultKey = "Default";

        public static int GetLayer(TeamType team)
        {
            return team switch
            {
                TeamType.Player => LayerMask.NameToLayer(PlayerBulletKey),
                TeamType.Enemy => LayerMask.NameToLayer(EnemyBulletKey),
                _ => LayerMask.NameToLayer(DefaultKey)
            };
        }
    }
}