using System;
using Game.Patrol;
using Game.Target;
using UnityEngine;

namespace Game
{
    public class Spider : 
        MonoBehaviour,
        MoveRequestComponent.IAction,
        MoveRequestComponent.ICondition,
        AttackRequestComponent.ICondition,
        AttackRequestComponent.IAction,
        DeathComponent.IAction,
        DeathComponent.ICondition,
        PatrolComponent.IAction,
        PatrolComponent.ICondition
    {
        [SerializeField]
        private float _damage;
        
        private MoveRequestComponent _moveRequestComponent;
        private MoveTransformComponent _moveTransformComponent;

        private PatrolComponent _patrolComponent;

        private HealthComponent _healthComponent;

        private AttackRequestComponent _attackRequestComponent;
        private ForceAttackComponent _attackComponent;

        private GroundedComponent _groundedComponent;

        [SerializeField]
        private CollisionComponent _collisionComponent;

        private DeathComponent _deathComponent;

        private FollowTargetComponent _followTargetComponent;

        private void Awake()
        {
            _moveRequestComponent = GetComponent<MoveRequestComponent>();
            _moveRequestComponent.SetAction(this);
            _moveRequestComponent.SetCondition(this);
            
            _moveTransformComponent = GetComponent<MoveTransformComponent>();

            _patrolComponent = GetComponent<PatrolComponent>();
            _patrolComponent.SetAction(this);
            _patrolComponent.SetCondition(this);

            _healthComponent = GetComponent<HealthComponent>();

            _attackRequestComponent = GetComponent<AttackRequestComponent>();
            _attackRequestComponent.SetAction(this);
            _attackRequestComponent.SetCondition(this);
            
            _attackComponent = GetComponent<ForceAttackComponent>();

            _groundedComponent = GetComponent<GroundedComponent>();

            _deathComponent = GetComponent<DeathComponent>();
            _deathComponent.SetCondition(this);
            _deathComponent.SetAction(this);

            _followTargetComponent = GetComponent<FollowTargetComponent>();
        }

        private void Start()
        {
            _followTargetComponent.SetTargetPoint(_patrolComponent.GetPoint());
        }

        private void OnEnable()
        {
            _collisionComponent.OnEntered += OnCollisionEntered;
            _healthComponent.OnDied += OnDied;
        }

        private void OnDisable()
        {
            _healthComponent.OnDied -= OnDied;
            _collisionComponent.OnEntered -= OnCollisionEntered;
        }

        private void FixedUpdate() 
            => _moveRequestComponent.SetMoveDirection(
                _followTargetComponent.GetDirectionToTarget());

        private void OnDied() => _deathComponent.RequestDeath();

        private void OnCollisionEntered(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out HealthComponent healthComponent))
            {
                healthComponent.TakeDamage(_damage);
                _attackRequestComponent.RequestAttack();
            }
        }
        
        void MoveRequestComponent.IAction.Invoke(Vector2 direction) 
            => _moveTransformComponent.Move(direction);

        bool MoveRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive && _groundedComponent.IsGrounded;

        bool AttackRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive && _groundedComponent.IsGrounded;

        [Obsolete]
        void AttackRequestComponent.IAction.Invoke() 
            => _attackComponent.Attack();

        void DeathComponent.IAction.Invoke() => Destroy(transform.parent.gameObject);

        bool DeathComponent.ICondition.Evaluate() => _healthComponent.IsDied;

        void PatrolComponent.IAction.Invoke() 
            => _followTargetComponent.SetTargetPoint(_patrolComponent.GetPoint());

        bool PatrolComponent.ICondition.Evaluate() 
            => _followTargetComponent.IsDestinationReached();
    }
}