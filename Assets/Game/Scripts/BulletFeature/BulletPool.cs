using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BulletPool : MonoBehaviour
    {
        private const int StartCount = 10;
        
        private readonly Stack<Bullet> _stack = new();

        [SerializeField] private BulletFactory _bulletFactory;

        [SerializeField] private Transform _container;

        private void Awake()
        {
            for (int i = 0; i < StartCount; i++)
            {
                Bullet bullet = _bulletFactory.Create(_container);
                bullet.gameObject.SetActive(false);
                _stack.Push(bullet);
            }
        }

        public Bullet Rent()
        {
            if (_stack.TryPop(out Bullet bullet))
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
   
            return _bulletFactory.Create(_container);
        }

        public void Push(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _stack.Push(bullet);
        }
    }
}