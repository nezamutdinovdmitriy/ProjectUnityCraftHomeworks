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
        PointProviderComponent.IAction,
        PointProviderComponent.ICondition
    {
        [SerializeField]
        private float _damage;
        
        private MoveRequestComponent _moveRequestComponent;
        private MoveTransformComponent _moveTransformComponent;
        
        private FollowTargetComponent _followTargetComponent;

        private PointProviderComponent pointProviderComponent;

        private HealthComponent _healthComponent;
        private DeathComponent _deathComponent;

        private AttackRequestComponent _attackRequestComponent;
        private ForceAttackComponent _attackComponent;

        private GroundedComponent _groundedComponent;
        
        private CollisionComponent _collisionComponent;
        
        private void Awake()
        {
            MovementBehaviourSetup();
            LifeCycleBehaviourSetup();
            AttackBehaviourSetup();

            _collisionComponent = GetComponent<CollisionComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
        }

        private void Start() 
            => _followTargetComponent.SetTargetPoint(pointProviderComponent.GetPoint());

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
        
        private void MovementBehaviourSetup()
        {
            _moveRequestComponent = GetComponent<MoveRequestComponent>();
            _moveRequestComponent.SetAction(this);
            _moveRequestComponent.SetCondition(this);
            
            _moveTransformComponent = GetComponent<MoveTransformComponent>();
            
            _followTargetComponent = GetComponent<FollowTargetComponent>();

            pointProviderComponent = GetComponent<PointProviderComponent>();
            pointProviderComponent.SetAction(this);
            pointProviderComponent.SetCondition(this);
        }

        private void LifeCycleBehaviourSetup()
        {
            _healthComponent = GetComponent<HealthComponent>();

            _deathComponent = GetComponent<DeathComponent>();
            _deathComponent.SetCondition(this);
            _deathComponent.SetAction(this);
        }

        private void AttackBehaviourSetup()
        {
            _attackRequestComponent = GetComponent<AttackRequestComponent>();
            _attackRequestComponent.SetAction(this);
            _attackRequestComponent.SetCondition(this);
            
            _attackComponent = GetComponent<ForceAttackComponent>();
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

        void PointProviderComponent.IAction.Invoke() 
            => _followTargetComponent.SetTargetPoint(pointProviderComponent.GetPoint());

        bool PointProviderComponent.ICondition.Evaluate() 
            => _followTargetComponent.IsDestinationReached();
    }
}