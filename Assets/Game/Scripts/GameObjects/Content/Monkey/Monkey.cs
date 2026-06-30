using System;
using Game.Target;
using UnityEngine;
using Zenject;

namespace Game.Scripts.GameObjects.Content.Monkey
{
    public class Monkey : 
        JumpRequestComponent.IAction,
        JumpRequestComponent.ICondition,
        DeathRequestComponent.IAction,
        DeathRequestComponent.ICondition,
        IInitializable,
        IDisposable,
        IFixedTickable
    {
        private readonly int _damage;
        
        private readonly JumpRequestComponent _jumpRequestComponent;
        private readonly JumpRigidbodyComponent _jumpRigidbodyComponent;

        private readonly HealthComponent _healthComponent;
        
        private readonly DeathRequestComponent _deathRequestComponent;
        
        private readonly GroundedComponent _groundedComponent;

        private readonly LookComponent _lookComponent;

        private readonly DetectTargetComponent _detectTargetComponent;
        
        private readonly CollisionComponent _collisionComponent;

        public Monkey(int damage,
            JumpRequestComponent jumpRequestComponent,
            JumpRigidbodyComponent jumpRigidbodyComponent,
            HealthComponent healthComponent,
            DeathRequestComponent deathRequestComponent,
            GroundedComponent groundedComponent,
            LookComponent lookComponent,
            DetectTargetComponent detectTargetComponent,
            CollisionComponent collisionComponent)
        {
            _damage = damage;
            _jumpRequestComponent = jumpRequestComponent;
            _jumpRigidbodyComponent = jumpRigidbodyComponent;
            _healthComponent = healthComponent;
            _deathRequestComponent = deathRequestComponent;
            _groundedComponent = groundedComponent;
            _lookComponent = lookComponent;
            _detectTargetComponent = detectTargetComponent;
            _collisionComponent = collisionComponent;
        }

        public void Initialize()
        {
            JumpBehaviourSetup();
            LifeCycleBehaviourSetup();
            
            _collisionComponent.OnEntered += OnCollisionEntered;
            _groundedComponent.OnGrounded += OnGrounded;
            _healthComponent.OnDied += OnDied;
        }

        public void Dispose()
        {
            _collisionComponent.OnEntered -= OnCollisionEntered;
            _groundedComponent.OnGrounded -= OnGrounded; 
            _healthComponent.OnDied -= OnDied;
        }

        public void FixedTick()
        {
            if (_detectTargetComponent.TryGetTarget(out GameObject target))
                _lookComponent.Look(target.transform);
        }

        private void OnCollisionEntered(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Entity entity)
                && entity.TryGet(out HealthComponent healthComponent))
                healthComponent.TakeDamage(_damage);
        }

        private void OnDied() => _deathRequestComponent.RequestDeath();

        private void OnGrounded(bool isGrounded)
        {
            if (isGrounded == false)
                return;
            
            _jumpRequestComponent.RequestJump();
        }

        private void LifeCycleBehaviourSetup()
        {
            _deathRequestComponent.SetAction(this);
            _deathRequestComponent.SetCondition(this);
        }
        
        private void JumpBehaviourSetup()
        {
            _jumpRequestComponent.SetAction(this);
            _jumpRequestComponent.SetCondition(this);
        }

        void JumpRequestComponent.IAction.Invoke() => _jumpRigidbodyComponent.Jump();

        bool JumpRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive && _groundedComponent.IsGrounded;

        void DeathRequestComponent.IAction.Invoke() => GameObject.Destroy(_collisionComponent.gameObject);

        bool DeathRequestComponent.ICondition.Evaluate() => _healthComponent.IsDied;
    }
}

// private void AttackBehaviourSetup()
// {
//     _attackRequestComponent = GetComponent<AttackRequestComponent>();
//     _attackRequestComponent.SetAction(this);
//     _attackRequestComponent.SetCondition(this);
//     
//     _forceAttackComponent = GetComponent<ForceAttackComponent>();
// }

// [Obsolete]
// void AttackRequestComponent.IAction.Invoke() => _forceAttackComponent.Attack();
//
// bool AttackRequestComponent.ICondition.Evaluate() 
//     => _healthComponent.IsAlive && _groundedComponent.IsGrounded;-


// private readonly AttackRequestComponent _attackRequestComponent;
// private readonly ForceAttackComponent _forceAttackComponent;