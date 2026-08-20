using System;
using UnityEngine;

namespace SampleGame
{
    public sealed class AttackComponent : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke(GameObject target);
        }

        public interface ICondition
        {
            bool IsMet(GameObject target);
        }

        public event Action OnFire;

        private ICondition _condition;
        private IAction _action;

        public void SetCondition(ICondition condition)
        {
            _condition = condition;
        }

        public void SetAction(IAction action)
        {
            _action = action;
        }

        public bool CanFire(GameObject target)
        {
            return _condition == null || _condition.IsMet(target);
        }

        public void Attack(GameObject target)
        {
            if (this.CanFire(target))
            {
                _action.Invoke(target);
                this.OnFire?.Invoke();
            }
        }
    }
}