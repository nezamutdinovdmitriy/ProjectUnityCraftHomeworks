using UnityEngine;

namespace Game
{
    [CreateAssetMenu(
        fileName = "BulletConfig",
        menuName = "Game/Bullet/BulletConfig")]
    public class BulletConfig : ScriptableObject
    {
        public int Damage;
        public float Speed;
    }
}