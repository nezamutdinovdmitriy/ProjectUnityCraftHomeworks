using System;
using Game.Patrol;
using Game.Scripts.GameObjects;
using Game.Target;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Spider : 
        MoveRequestComponent.IAction,
        MoveRequestComponent.ICondition,
        AttackRequestComponent.ICondition,
        AttackRequestComponent.IAction,
        DeathRequestComponent.IAction,
        DeathRequestComponent.ICondition,
        PatrolComponent.ICondition,
        IInitializable,
        IDisposable
    {
        private readonly int _damage;
        
        private readonly MoveRequestComponent _moveRequestComponent;
        private readonly MoveTransformComponent _moveTransformComponent;
        
        private readonly FollowTargetComponent _followTargetComponent;

        private readonly PatrolComponent _pointProviderComponent;

        private readonly HealthComponent _healthComponent;
        private readonly DeathRequestComponent _deathRequestComponent;

        private readonly AttackRequestComponent _attackRequestComponent;
        private readonly ForceAttackComponent _attackComponent;

        private readonly GroundedComponent _groundedComponent;
        
        private readonly CollisionComponent _collisionComponent;

        private readonly Transform _transform;

        public Spider(
            int damage, 
            MoveRequestComponent moveRequestComponent, 
            MoveTransformComponent moveTransformComponent, 
            FollowTargetComponent followTargetComponent, 
            PatrolComponent pointProviderComponent, 
            HealthComponent healthComponent, 
            DeathRequestComponent deathRequestComponent, 
            AttackRequestComponent attackRequestComponent, 
            ForceAttackComponent attackComponent, 
            GroundedComponent groundedComponent, 
            CollisionComponent collisionComponent, 
            Transform transform)
        {
            _damage = damage;
            _moveRequestComponent = moveRequestComponent;
            _moveTransformComponent = moveTransformComponent;
            _followTargetComponent = followTargetComponent;
            _pointProviderComponent = pointProviderComponent;
            _healthComponent = healthComponent;
            _deathRequestComponent = deathRequestComponent;
            _attackRequestComponent = attackRequestComponent;
            _attackComponent = attackComponent;
            _groundedComponent = groundedComponent;
            _collisionComponent = collisionComponent;
            _transform = transform;
        }

        public void Initialize()
        {
            MovementBehaviourSetup();
            LifeCycleBehaviourSetup();
            AttackBehaviourSetup();
            
            _collisionComponent.OnEntered += OnCollisionEntered;
            _healthComponent.OnDied += OnDied;
        }

        public void Dispose()
        {
            _healthComponent.OnDied -= OnDied;
            _collisionComponent.OnEntered -= OnCollisionEntered;
        }

        private void OnDied() 
            => _deathRequestComponent.RequestDeath();

        private void OnCollisionEntered(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IEntity entity)
                && entity.TryGet(out HealthComponent health))
            {
                health.TakeDamage(_damage);
                _attackRequestComponent.RequestAttack();
            }
        }
        
        private void MovementBehaviourSetup()
        {
            _moveRequestComponent.SetAction(this);
            _moveRequestComponent.SetCondition(this);
            
            _pointProviderComponent.SetCondition(this);
        }

        private void LifeCycleBehaviourSetup()
        {
            _deathRequestComponent.SetCondition(this);
            _deathRequestComponent.SetAction(this);
        }

        private void AttackBehaviourSetup()
        {
            _attackRequestComponent.SetAction(this);
            _attackRequestComponent.SetCondition(this);
        }
        
        void MoveRequestComponent.IAction.Invoke(Vector2 direction) 
            => _moveTransformComponent.Move(direction);

        bool MoveRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive && _groundedComponent.IsGrounded;

        bool AttackRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive && _groundedComponent.IsGrounded;

        void AttackRequestComponent.IAction.Invoke() 
            => _attackComponent.Attack();

        void DeathRequestComponent.IAction.Invoke() 
            => GameObject.Destroy(_transform.parent.gameObject);

        bool DeathRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsDied;

        bool PatrolComponent.ICondition.Evaluate() 
            => _followTargetComponent.IsDestinationReached();
    }
}