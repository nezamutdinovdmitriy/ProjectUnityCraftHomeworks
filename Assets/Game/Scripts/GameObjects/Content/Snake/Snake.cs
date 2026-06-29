using System;
using Game.Target;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MoveRequestComponent), typeof(MoveTransformComponent))]
    [RequireComponent(typeof(DetectTargetComponent), typeof(FollowTargetComponent))]
    [RequireComponent(typeof(HealthComponent), typeof(DeathComponent))]
    [RequireComponent(typeof(AttackRequestComponent), typeof(ForceAttackComponent))]
    [RequireComponent(typeof(LookComponent), typeof(CollisionComponent))]
    public class Snake : MonoBehaviour,
        MoveRequestComponent.IAction,
        MoveRequestComponent.ICondition,
        AttackRequestComponent.IAction,
        AttackRequestComponent.ICondition,
        DeathComponent.IAction,
        DeathComponent.ICondition
    {
        [SerializeField]
        private int _damage;
        
        private MoveRequestComponent _moveRequestComponent;
        private MoveTransformComponent _moveTransformComponent;

        private DetectTargetComponent _detectTargetComponent;
        private FollowTargetComponent _followTargetComponent;
        
        private HealthComponent _healthComponent;
        private DeathComponent _deathComponent;

        private AttackRequestComponent _attackRequestComponent;
        private ForceAttackComponent _forceAttackComponent;
        
        private LookComponent _lookComponent;
        
        private CollisionComponent _collisionComponent;

        private HealthComponent _currentTargetToDamage;
        
        private void Awake()
        {
            MovementBehaviourSetup();
            AttackBehaviourSetup();
            LifeCycleBehaviourSetup();

            _collisionComponent = GetComponent<CollisionComponent>();
            _lookComponent = GetComponent<LookComponent>();
        }
        
        private void OnEnable()
        {
            _healthComponent.OnDied += OnDied;
            _collisionComponent.OnEntered += OnCollisionEntered;
        }

        private void OnDisable()
        {
            _healthComponent.OnDied -= OnDied;
            _collisionComponent.OnEntered -= OnCollisionEntered;
        }

        private void FixedUpdate()
        {
            if (_detectTargetComponent.TryGetTarget(out GameObject target))
            {
                _followTargetComponent.SetTargetPoint(target.transform.position);
                
                if(_followTargetComponent.IsDestinationReached() == false)
                    _moveRequestComponent.SetMoveDirection(_followTargetComponent.GetDirectionToTarget());
            }
        }

        private void OnDied() => _deathComponent.RequestDeath();

        private void OnCollisionEntered(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out HealthComponent healthComponent))
            {
                _currentTargetToDamage = healthComponent;
                _attackRequestComponent.RequestAttack();
            }
        }
        
        private void LifeCycleBehaviourSetup()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _deathComponent = GetComponent<DeathComponent>();
            _deathComponent.SetAction(this);
            _deathComponent.SetCondition(this);
        }

        private void AttackBehaviourSetup()
        {
            _attackRequestComponent = GetComponent<AttackRequestComponent>();
            _attackRequestComponent.SetAction(this);
            _attackRequestComponent.SetCondition(this);
            
            _forceAttackComponent = GetComponent<ForceAttackComponent>();
        }

        private void MovementBehaviourSetup()
        {
            _moveRequestComponent = GetComponent<MoveRequestComponent>();
            _moveRequestComponent.SetAction(this);
            _moveRequestComponent.SetCondition(this);
            
            _moveTransformComponent = GetComponent<MoveTransformComponent>();
            _detectTargetComponent = GetComponent<DetectTargetComponent>();
            _followTargetComponent = GetComponent<FollowTargetComponent>();
        }
        
        void MoveRequestComponent.IAction.Invoke(Vector2 direction)
        {
            _lookComponent.Look(direction.x);
            _moveTransformComponent.Move(direction);
        }

        bool MoveRequestComponent.ICondition.Evaluate() => _healthComponent.IsAlive;

        [Obsolete]
        void AttackRequestComponent.IAction.Invoke()
        {
            _currentTargetToDamage.TakeDamage(_damage);
            _forceAttackComponent.Attack();
        }

        bool AttackRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive && _currentTargetToDamage != null;

        void DeathComponent.IAction.Invoke() => Destroy(gameObject);

        bool DeathComponent.ICondition.Evaluate() => _healthComponent.IsDied;
    }
}