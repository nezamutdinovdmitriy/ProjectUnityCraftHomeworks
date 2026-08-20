using UnityEngine;

namespace SampleGame
{
    public class Character : MonoBehaviour,
        MoveComponent.ICondition,
        MoveComponent.IAction,
        AttackComponent.ICondition,
        AttackComponent.IAction
    {
        [Header("Move")]
        [SerializeField]
        private MoveComponent _moveComponent;

        [SerializeField]
        private MoveTransformComponent _moveTransformComponent;

        [SerializeField]
        private RotateTransformComponent _rotateTransformComponent;

        [Header("Attack")]
        [SerializeField]
        private AttackComponent _attackComponent;

        [SerializeField]
        private Weapon _weapon;

        [Header("Health")]
        [SerializeField]
        private HealthComponent _healthComponent;

        [SerializeField]
        private float _deathDelay = 1.0f;

        private void Awake()
        {
            _moveComponent.SetCondition(this);
            _moveComponent.SetAction(this);
            
            _attackComponent.SetCondition(this);
            _attackComponent.SetAction(this);
        }

        private void OnEnable()
        {
            _healthComponent.OnDeath += this.OnDeath;
        }

        private void OnDisable()
        {
            _healthComponent.OnDeath += this.OnDeath;
        }

        private void OnDeath()
        {
            Destroy(this.gameObject, _deathDelay);
        }

        bool MoveComponent.ICondition.IsMet(Vector3 direction)
        {
            return _healthComponent.IsAlive;
        }

        void MoveComponent.IAction.Invoke(Vector3 direction, float deltaTime)
        {
            _moveTransformComponent.MoveStep(direction, deltaTime);
            _rotateTransformComponent.RotateTowards(direction, deltaTime);
        }

        bool AttackComponent.ICondition.IsMet(GameObject target)
        {
            return _healthComponent.IsAlive &&
                   target && target.TryGetComponent(out HealthComponent health) && health.IsAlive &&
                   _weapon.CanFire(target);
        }

        void AttackComponent.IAction.Invoke(GameObject target)
        {
            _weapon.Fire(target);
            _rotateTransformComponent.LookAt(target);
        }
    }
}