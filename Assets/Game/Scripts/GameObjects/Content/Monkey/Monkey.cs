using System;
using Game.Target;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Monkey
{
    public class Monkey : MonoBehaviour,
        AttackRequestComponent.IAction,
        AttackRequestComponent.ICondition,
        JumpRequestComponent.IAction,
        JumpRequestComponent.ICondition,
        DeathComponent.IAction,
        DeathComponent.ICondition
    {
        [SerializeField]
        private int _damage;
        
        private JumpRequestComponent _jumpRequestComponent;
        private JumpRigidbodyComponent _jumpRigidbodyComponent;

        private AttackRequestComponent _attackRequestComponent;
        
        private ForceAttackComponent _forceAttackComponent;

        private HealthComponent _healthComponent;

        private GroundedComponent _groundedComponent;

        private LookComponent _lookComponent;

        private DetectTargetComponent _detectTargetComponent;
        
        private CollisionComponent _collisionComponent;

        private DeathComponent _deathComponent;

        private void Awake()
        {
            _jumpRequestComponent = GetComponent<JumpRequestComponent>();
            _jumpRequestComponent.SetAction(this);
            _jumpRequestComponent.SetCondition(this);
            
            _jumpRigidbodyComponent = GetComponent<JumpRigidbodyComponent>();
            
            _attackRequestComponent = GetComponent<AttackRequestComponent>();
            _attackRequestComponent.SetAction(this);
            _attackRequestComponent.SetCondition(this);
            
            _forceAttackComponent = GetComponent<ForceAttackComponent>();
            
            _healthComponent = GetComponent<HealthComponent>();
            
            _groundedComponent = GetComponent<GroundedComponent>();
            
            _lookComponent = GetComponent<LookComponent>();

            _collisionComponent = GetComponent<CollisionComponent>();

            _detectTargetComponent = GetComponent<DetectTargetComponent>();

            _deathComponent = GetComponent<DeathComponent>();
            _deathComponent.SetAction(this);
            _deathComponent.SetCondition(this);
        }

        private void OnEnable()
        {
            _collisionComponent.OnEntered += OnCollisionEntered;
            _groundedComponent.OnGrounded += OnGrounded;
            _healthComponent.OnDied += OnDied;
        }

        private void OnDisable()
        {
            _collisionComponent.OnEntered -= OnCollisionEntered;
            _groundedComponent.OnGrounded -= OnGrounded; 
            _healthComponent.OnDied -= OnDied;
        }

        private void FixedUpdate()
        {
            if (_detectTargetComponent.TryGetTarget(out GameObject target))
                _lookComponent.Look(target.transform);
        }

        private void OnCollisionEntered(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out HealthComponent healthComponent))
                healthComponent.TakeDamage(_damage);
        }

        private void OnDied() => _deathComponent.RequestDeath();

        private void OnGrounded(bool isGrounded)
        {
            if (isGrounded == false)
                return;
            
            _attackRequestComponent.RequestAttack();
            _jumpRequestComponent.RequestJump();
        }

        [Obsolete]
        void AttackRequestComponent.IAction.Invoke() => _forceAttackComponent.Attack();

        bool AttackRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive && _groundedComponent.IsGrounded;

        void JumpRequestComponent.IAction.Invoke() => _jumpRigidbodyComponent.Jump();

        bool JumpRequestComponent.ICondition.Evaluate() 
            => _healthComponent.IsAlive && _groundedComponent.IsGrounded;

        void DeathComponent.IAction.Invoke() => Destroy(gameObject);

        bool DeathComponent.ICondition.Evaluate() => _healthComponent.IsDied;
    }
}