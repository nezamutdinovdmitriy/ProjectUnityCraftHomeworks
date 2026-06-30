using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Character :
        MoveRequestComponent.IAction,
        MoveRequestComponent.ICondition,
        JumpRequestComponent.IAction,
        JumpRequestComponent.ICondition,
        DeathRequestComponent.IAction,
        DeathRequestComponent.ICondition,
        IInitializable,
        IDisposable
    {
        private readonly Rigidbody2D _rigidbody;
        
        private readonly MoveRequestComponent _moveRequestComponent;
        private readonly MoveTransformComponent _moveTransformComponent;
        private readonly LookComponent _lookComponent;
        
        private readonly JumpRequestComponent _jumpRequestComponent;
        private readonly JumpRigidbodyComponent _jumpRigidbodyComponent;
        
        private readonly HealthComponent _healthComponent;
        private readonly DeathRequestComponent _deathRequestComponent;
        
        private readonly GroundedComponent _groundedComponent;

        public Character(Rigidbody2D rigidbody, 
            MoveRequestComponent moveRequestComponent,
            MoveTransformComponent moveTransformComponent,
            LookComponent lookComponent, 
            JumpRequestComponent jumpRequestComponent,
            JumpRigidbodyComponent jumpRigidbodyComponent,
            HealthComponent healthComponent,
            DeathRequestComponent deathRequestComponent,
            GroundedComponent groundedComponent)
        {
            _rigidbody = rigidbody;
            _moveRequestComponent = moveRequestComponent;
            _moveTransformComponent = moveTransformComponent;
            _lookComponent = lookComponent;
            _jumpRequestComponent = jumpRequestComponent;
            _jumpRigidbodyComponent = jumpRigidbodyComponent;
            _healthComponent = healthComponent;
            _deathRequestComponent = deathRequestComponent;
            _groundedComponent = groundedComponent;
        }
        
        public void Initialize()
        {
            LifeCycleBehaviourSetup();
            MovementBehaviourSetup();
            
            _healthComponent.OnDied += OnDied;
        }
        
        public void Dispose() => _healthComponent.OnDied -= OnDied;

        private void LifeCycleBehaviourSetup()
        {
            _deathRequestComponent.SetAction(this);
            _deathRequestComponent.SetCondition(this);
        }

        private void MovementBehaviourSetup()
        {
            _moveRequestComponent.SetAction(this);
            _moveRequestComponent.SetCondition(this);

            _jumpRequestComponent.SetAction(this);
            _jumpRequestComponent.SetCondition(this);
        }
        
        private void OnDied()
        {
            _rigidbody.simulated = false;
            _deathRequestComponent.RequestDeath();
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

        void DeathRequestComponent.IAction.Invoke() => GameObject.Destroy(_rigidbody.gameObject);
        bool DeathRequestComponent.ICondition.Evaluate() => _healthComponent.IsDied;
    }
}
// [Header("Attack")] [SerializeField]
        // private GameObject _pushAttack;
        // private AttackRequestComponent _pushAttackRequestComponent;
        //
        // [SerializeField]
        // private GameObject _tossAttack;
        // private AttackRequestComponent _tossAttackRequestComponent;
        
        // public void MainAttack() => _pushAttackRequestComponent.RequestAttack();
        // public void AdditionalAttack() => _tossAttackRequestComponent.RequestAttack();
        
        // private void AttackBehaviorSetup()
        // {
        //     _pushAttackRequestComponent = _pushAttack.GetComponent<AttackRequestComponent>();
        //
        //     PushAttack pushAttack = new(this);
        //     _pushAttackRequestComponent.SetCondition(pushAttack);
        //     _pushAttackRequestComponent.SetAction(pushAttack);
        //
        //     _tossAttackRequestComponent = _tossAttack.GetComponent<AttackRequestComponent>();
        //
        //     TossAttack tossAttack = new(this);
        //     _tossAttackRequestComponent.SetCondition(tossAttack);
        //     _tossAttackRequestComponent.SetAction(tossAttack);
        // }
        
        // private class PushAttack : AttackRequestComponent.IAction, AttackRequestComponent.ICondition
        // {
        //     private readonly Character _parent;
        //
        //     public PushAttack(Character parent) => _parent = parent;
        //
        //     [Obsolete]
        //     void AttackRequestComponent.IAction.Invoke()
        //         => _parent._pushAttack.GetComponent<ForceAttackComponent>().Attack();
        //
        //     bool AttackRequestComponent.ICondition.Evaluate()
        //     {
        //         return _parent._tossAttackRequestComponent.IsRequested == false
        //                && _parent._healthComponent.IsAlive;
        //     }
        // }
        //
        // private class TossAttack : AttackRequestComponent.IAction, AttackRequestComponent.ICondition
        // {
        //     private readonly Character _parent;
        //
        //     public TossAttack(Character parent) => _parent = parent;
        //
        //     [Obsolete]
        //     void AttackRequestComponent.IAction.Invoke()
        //         => _parent._tossAttack.GetComponent<ForceAttackComponent>().Attack();
        //
        //     bool AttackRequestComponent.ICondition.Evaluate()
        //     {
        //         return _parent._pushAttackRequestComponent.IsRequested == false
        //                && _parent._healthComponent.IsAlive
        //                && _parent._groundedComponent.IsGrounded;
        //     }
        // }