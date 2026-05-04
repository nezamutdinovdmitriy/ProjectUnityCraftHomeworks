using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BulletPool : MonoBehaviour
    {
        private readonly Stack<Bullet> _stack = new();

        [SerializeField]
        private BulletFactory _bulletFactory;

        [SerializeField]
        private Transform _container;

        public Bullet Rent()
        {
            if (_stack.TryPop(out Bullet bullet))
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
            else
            {
                return _bulletFactory.Create(_container);
            }
            
        }

        public void Push(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _stack.Push(bullet);
        }
    }
}