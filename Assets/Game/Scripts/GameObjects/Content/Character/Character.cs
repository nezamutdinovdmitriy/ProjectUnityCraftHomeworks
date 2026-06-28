using System;
using UnityEngine;

namespace Game
{
    public class Character :
        MonoBehaviour,
        MoveRequestComponent.IAction,
        MoveRequestComponent.ICondition,
        JumpRequestComponent.IAction,
        JumpRequestComponent.ICondition,
        DeathComponent.IAction,
        DeathComponent.ICondition,
        IPlayerAttacks
    {
        private Rigidbody2D _rigidbody;

        [Header("MoveComponents")]
        private MoveRequestComponent _moveRequestComponent;
        private MoveTransformComponent _moveTransformComponent;
        private LookComponent _lookComponent;

        [Header("JumpComponents")]
        private JumpRequestComponent _jumpRequestComponent;
        private JumpRigidbodyComponent _jumpRigidbodyComponent;

        [Header("HealthComponents")]
        private HealthComponent _healthComponent;
        private DeathComponent _deathComponent;

        [Header("Attack")] [SerializeField]
        private GameObject _pushAttack;
        private AttackRequestComponent _pushAttackRequestComponent;

        [SerializeField]
        private GameObject _tossAttack;
        private AttackRequestComponent _tossAttackRequestComponent;

        [Header("OtherComponents")]
        private GroundedComponent _groundedComponent;

        [Obsolete]
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _groundedComponent = GetComponent<GroundedComponent>();
            
            MovementBehaviourSetup();
            LifeCycleBehaviourSetup();
            AttackBehaviorSetup();
        }

        private void OnEnable()
        {
            _healthComponent.OnDied += OnDied;
        }

        private void OnDisable()
        {
            _healthComponent.OnDied -= OnDied;
        }

        private void OnDied()
        {
            _rigidbody.simulated = false;
            _deathComponent.RequestDeath();
        }

        public void MainAttack() => _pushAttackRequestComponent.RequestAttack();
        public void AdditionalAttack() => _tossAttackRequestComponent.RequestAttack();
        
        private void LifeCycleBehaviourSetup()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _deathComponent = GetComponent<DeathComponent>();
            _deathComponent.SetAction(this);
            _deathComponent.SetCondition(this);
        }

        private void MovementBehaviourSetup()
        {
            _lookComponent = GetComponent<LookComponent>();
            
            _moveRequestComponent = GetComponent<MoveRequestComponent>();
            _moveRequestComponent.SetAction(this);
            _moveRequestComponent.SetCondition(this);
            
            _moveTransformComponent = GetComponent<MoveTransformComponent>();
            
            _jumpRequestComponent = GetComponent<JumpRequestComponent>();
            _jumpRequestComponent.SetAction(this);
            _jumpRequestComponent.SetCondition(this);

            _jumpRigidbodyComponent = GetComponent<JumpRigidbodyComponent>();
        }

        private void AttackBehaviorSetup()
        {
            _pushAttackRequestComponent = _pushAttack.GetComponent<AttackRequestComponent>();

            PushAttack pushAttack = new(this);
            _pushAttackRequestComponent.SetCondition(pushAttack);
            _pushAttackRequestComponent.SetAction(pushAttack);

            _tossAttackRequestComponent = _tossAttack.GetComponent<AttackRequestComponent>();

            TossAttack tossAttack = new(this);
            _tossAttackRequestComponent.SetCondition(tossAttack);
            _tossAttackRequestComponent.SetAction(tossAttack);
        }

        void MoveRequestComponent.IAction.Invoke(Vector2 direction)
        {
            _moveTransformComponent.Move(direction);
            _lookComponent.Look(direction.x);
        }

        bool MoveRequestComponent.ICondition.Evaluate()
            => _healthComponent.IsAlive;

        void JumpRequestComponent.IAction.Invoke() => _jumpRigidbodyComponent.Jump();
        bool JumpRequestComponent.ICondition.Evaluate()
            => _healthComponent.IsAlive && _groundedComponent.IsGrounded;

        void DeathComponent.IAction.Invoke() => Destroy(gameObject);
        bool DeathComponent.ICondition.Evaluate() => _healthComponent.IsDied;

        private class PushAttack : AttackRequestComponent.IAction, AttackRequestComponent.ICondition
        {
            private readonly Character _parent;

            public PushAttack(Character parent) => _parent = parent;

            [Obsolete]
            void AttackRequestComponent.IAction.Invoke()
                => _parent._pushAttack.GetComponent<ForceAttackComponent>().Attack();

            bool AttackRequestComponent.ICondition.Evaluate()
            {
                return _parent._tossAttackRequestComponent.Requested == false
                       && _parent._healthComponent.IsAlive;
            }
        }

        private class TossAttack : AttackRequestComponent.IAction, AttackRequestComponent.ICondition
        {
            private readonly Character _parent;

            public TossAttack(Character parent) => _parent = parent;

            [Obsolete]
            void AttackRequestComponent.IAction.Invoke()
                => _parent._tossAttack.GetComponent<ForceAttackComponent>().Attack();

            bool AttackRequestComponent.ICondition.Evaluate()
            {
                return _parent._pushAttackRequestComponent.Requested == false
                       && _parent._healthComponent.IsAlive
                       && _parent._groundedComponent.IsGrounded;
            }
        }
    }
}