using System;
using Game.Target;
using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Monkey
{
    [RequireComponent(typeof(JumpRequestComponent), typeof(JumpRigidbodyComponent))]
    [RequireComponent(typeof(AttackRequestComponent), typeof(ForceAttackComponent))]
    [RequireComponent(typeof(HealthComponent), typeof(DeathComponent))]
    [RequireComponent(typeof(GroundedComponent), typeof(LookComponent))]
    [RequireComponent(typeof(DetectTargetComponent), typeof(CollisionComponent))]
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
        private DeathComponent _deathComponent;
        
        private GroundedComponent _groundedComponent;

        private LookComponent _lookComponent;

        private DetectTargetComponent _detectTargetComponent;
        
        private CollisionComponent _collisionComponent;

        private void Awake()
        {
            JumpBehaviourSetup();
            AttackBehaviourSetup();
            LifeCycleBehaviourSetup();
            TargetBehaviourSetup();

            _groundedComponent = GetComponent<GroundedComponent>();
            _collisionComponent = GetComponent<CollisionComponent>();
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
        
        private void TargetBehaviourSetup()
        {
            _lookComponent = GetComponent<LookComponent>();
            _detectTargetComponent = GetComponent<DetectTargetComponent>();
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

        private void JumpBehaviourSetup()
        {
            _jumpRequestComponent = GetComponent<JumpRequestComponent>();
            _jumpRequestComponent.SetAction(this);
            _jumpRequestComponent.SetCondition(this);
            
            _jumpRigidbodyComponent = GetComponent<JumpRigidbodyComponent>();
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