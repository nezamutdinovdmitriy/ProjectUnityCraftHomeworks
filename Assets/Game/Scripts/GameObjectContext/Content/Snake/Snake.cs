using System;
using GameObjects.Components;
using UnityEngine;
using Zenject;

namespace GameObjects.Content
{
    public class Snake : 
        MoveRequestComponent.IAction,
        MoveRequestComponent.ICondition,
        AttackRequestComponent.IAction,
        AttackRequestComponent.ICondition,
        DeathRequestComponent.IAction,
        DeathRequestComponent.ICondition,
        FollowTargetComponent.ICondition,
        IInitializable,
        IDisposable
    {
        private readonly int _damage;
        
        private readonly MoveRequestComponent _moveRequestComponent;
        private readonly MoveTransformComponent _moveTransformComponent;

        private readonly TargetComponent _targetComponent;
        private readonly FollowTargetComponent _followTargetComponent;
        
        private readonly HealthComponent _healthComponent;
        private readonly DeathRequestComponent _deathRequestComponent;

        private readonly AttackRequestComponent _attackRequestComponent;
        private readonly ForceAttackComponent _forceAttackComponent;
        
        private readonly LookComponent _lookComponent;
        
        private readonly CollisionComponent _collisionComponent;
        
        private HealthComponent _currentTargetToDamage;

        public Snake(int damage,
            MoveRequestComponent moveRequestComponent,
            MoveTransformComponent moveTransformComponent,
            FollowTargetComponent followTargetComponent,
            HealthComponent healthComponent,
            DeathRequestComponent deathRequestComponent,
            AttackRequestComponent attackRequestComponent,
            ForceAttackComponent forceAttackComponent, 
            LookComponent lookComponent,
            CollisionComponent collisionComponent,
            TargetComponent targetComponent)
        {
            _damage = damage;
            _moveRequestComponent = moveRequestComponent;
            _moveTransformComponent = moveTransformComponent;
            _followTargetComponent = followTargetComponent;
            _healthComponent = healthComponent;
            _deathRequestComponent = deathRequestComponent;
            _attackRequestComponent = attackRequestComponent;
            _forceAttackComponent = forceAttackComponent;
            _lookComponent = lookComponent;
            _collisionComponent = collisionComponent;
            _targetComponent = targetComponent;
        }

        public void Initialize()
        {
            _followTargetComponent.SetCondition(this);
            
            MovementBehaviourSetup();
            AttackBehaviourSetup();
            LifeCycleBehaviourSetup();
            
            _healthComponent.OnDied += OnDied;
            _collisionComponent.OnEntered += OnCollisionEntered;
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
            if (collision.gameObject.TryGetComponent(out Entity entity)
                && entity.TryGet(out HealthComponent health))
            {
                _currentTargetToDamage = health;
                _attackRequestComponent.RequestAttack();
            }
        }
        
        private void LifeCycleBehaviourSetup()
        {
            _deathRequestComponent.SetAction(this);
            _deathRequestComponent.SetCondition(this);
        }

        private void AttackBehaviourSetup()
        {
            _attackRequestComponent.SetAction(this);
            _attackRequestComponent.SetCondition(this);
        }

        private void MovementBehaviourSetup()
        {
            _moveRequestComponent.SetAction(this);
            _moveRequestComponent.SetCondition(this);
        }
        
        void MoveRequestComponent.IAction.Invoke(Vector2 direction)
        {
            _lookComponent.Look(direction.x);
            _moveTransformComponent.Move(direction);
        }

        bool MoveRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive;

        void AttackRequestComponent.IAction.Invoke()
        {
            _currentTargetToDamage.TakeDamage(_damage);
            _forceAttackComponent.Attack();
        }

        bool AttackRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive && _currentTargetToDamage != null;

        void DeathRequestComponent.IAction.Invoke() 
            => GameObject.Destroy(_collisionComponent.gameObject);

        bool DeathRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsDied;

        bool FollowTargetComponent.ICondition.Evaluate() 
            => _targetComponent.Target != null;
    }
}