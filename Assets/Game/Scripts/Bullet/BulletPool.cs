using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField]
        private BulletFactory _bulletFactory;

        [SerializeField]
        private int _objectLimit = 10;

        private Stack<Bullet> _stack;

        private void Awake()
        {
            _stack = new Stack<Bullet>(capacity: _objectLimit);
        }

        public Bullet Rent()
        {
            if (_stack.TryPop(out Bullet bullet))
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
            else
                return _bulletFactory.Create();
        }

        public void Return(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _stack.Push(bullet);
        }
    }
}