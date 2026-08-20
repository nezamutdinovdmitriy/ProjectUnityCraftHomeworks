using System;
using UnityEngine;

namespace SampleGame
{
    public abstract class Weapon : MonoBehaviour
    {
        public event Action OnFire;
        
        public abstract bool CanFire(GameObject target);

        public void Fire(GameObject target)
        {
            if (this.CanFire(target))
            {
                this.ProcessFire(target);
                this.OnFire?.Invoke();
            }
        }

        protected abstract void ProcessFire(GameObject target);
    }
}