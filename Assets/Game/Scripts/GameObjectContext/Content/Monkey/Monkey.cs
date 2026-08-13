using System;
using Game.Scripts.GameObjects;
using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
{
    public class Monkey :
        JumpRequestComponent.IAction,
        JumpRequestComponent.ICondition,
        DeathRequestComponent.IAction,
        DeathRequestComponent.ICondition,
        AttackRequestComponent.IAction,
        AttackRequestComponent.ICondition,
        IInitializable,
        IDisposable,
        IFixedTickable
    {
        private readonly int _damage;

        private readonly AttackRequestComponent _attackRequestComponent;
        private readonly ForceAttackComponent _forceAttackComponent;

        private readonly JumpRequestComponent _jumpRequestComponent;
        private readonly JumpRigidbodyComponent _jumpRigidbodyComponent;

        private readonly HealthComponent _healthComponent;
        private readonly DeathRequestComponent _deathRequestComponent;

        private readonly GroundedComponent _groundedComponent;

        private readonly LookComponent _lookComponent;

        private readonly CollisionComponent _collisionComponent;

        private readonly TargetComponent _targetComponent;

        public Monkey(int damage,
            JumpRequestComponent jumpRequestComponent,
            JumpRigidbodyComponent jumpRigidbodyComponent,
            HealthComponent healthComponent,
            DeathRequestComponent deathRequestComponent,
            GroundedComponent groundedComponent,
            LookComponent lookComponent,
            CollisionComponent collisionComponent, 
            AttackRequestComponent attackRequestComponent, 
            ForceAttackComponent forceAttackComponent, 
            TargetComponent targetComponent)
        {
            _damage = damage;
            _jumpRequestComponent = jumpRequestComponent;
            _jumpRigidbodyComponent = jumpRigidbodyComponent;
            _healthComponent = healthComponent;
            _deathRequestComponent = deathRequestComponent;
            _groundedComponent = groundedComponent;
            _lookComponent = lookComponent;
            _collisionComponent = collisionComponent;
            _attackRequestComponent = attackRequestComponent;
            _forceAttackComponent = forceAttackComponent;
            _targetComponent = targetComponent;
        }

        public void Initialize()
        {
            AttackBehaviourSetup();
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
            if(_targetComponent.Target != null)
                _lookComponent.Look(_targetComponent.Target.transform);
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

            _attackRequestComponent.RequestAttack();
            _jumpRequestComponent.RequestJump();
        }

        private void AttackBehaviourSetup()
        {
            _attackRequestComponent.SetAction(this);
            _attackRequestComponent.SetCondition(this);
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

        void JumpRequestComponent.IAction.Invoke() 
            => _jumpRigidbodyComponent.Jump();

        bool JumpRequestComponent.ICondition.Evaluate()
            => _healthComponent.IsAlive && _groundedComponent.IsGrounded;

        void DeathRequestComponent.IAction.Invoke() 
            => GameObject.Destroy(_collisionComponent.gameObject);

        bool DeathRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsDied;

        void AttackRequestComponent.IAction.Invoke() 
            => _forceAttackComponent.Attack();

        bool AttackRequestComponent.ICondition.Evaluate()
            => _healthComponent.IsAlive && _groundedComponent.IsGrounded;
    }
}