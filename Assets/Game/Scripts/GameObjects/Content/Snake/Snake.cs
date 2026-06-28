using System;
using Game.Target;
using UnityEngine;

namespace Game
{
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
        
        private DetectTargetComponent _detectTargetComponent;

        private MoveRequestComponent _moveRequestComponent;
        private MoveTransformComponent _moveTransformComponent;

        private HealthComponent _healthComponent;

        private FollowTargetComponent _followTargetComponent;

        private CollisionComponent _collisionComponent;

        private AttackRequestComponent _attackRequestComponent;
        private ForceAttackComponent _forceAttackComponent;

        private HealthComponent _currentTargetToDamage;

        private LookComponent _lookComponent;

        private DeathComponent _deathComponent;
        
        private void Awake()
        {
            _detectTargetComponent = GetComponent<DetectTargetComponent>();
            
            _moveRequestComponent = GetComponent<MoveRequestComponent>();
            _moveRequestComponent.SetAction(this);
            _moveRequestComponent.SetCondition(this);
            
            _moveTransformComponent = GetComponent<MoveTransformComponent>();

            _healthComponent = GetComponent<HealthComponent>();

            _followTargetComponent = GetComponent<FollowTargetComponent>();

            _collisionComponent = GetComponent<CollisionComponent>();

            _attackRequestComponent = GetComponent<AttackRequestComponent>();
            _attackRequestComponent.SetAction(this);
            _attackRequestComponent.SetCondition(this);
            
            _forceAttackComponent = GetComponent<ForceAttackComponent>();

            _lookComponent = GetComponent<LookComponent>();

            _deathComponent = GetComponent<DeathComponent>();
            _deathComponent.SetAction(this);
            _deathComponent.SetCondition(this);
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