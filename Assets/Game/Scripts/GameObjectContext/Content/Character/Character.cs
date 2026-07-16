using System;
using Game.Scripts.GameObjects;
using Game.Scripts.GameObjects.GameSystems.Attack;
using Game.Scripts.SceneContext;
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
        IDisposable,
        IPushComponent,
        ITossComponent
    {
        private readonly Rigidbody2D _rigidbody;

        private readonly AttackRequestComponent _pushAttackRequestComponent;
        private readonly AttackRequestComponent _tossAttackRequestComponent;
        
        private readonly ForceAttackComponent _pushAttackComponent;
        private readonly ForceAttackComponent _tossAttackComponent;
        
        private readonly MoveRequestComponent _moveRequestComponent;
        private readonly MoveTransformComponent _moveTransformComponent;
        private readonly LookComponent _lookComponent;
        
        private readonly JumpRequestComponent _jumpRequestComponent;
        private readonly JumpRigidbodyComponent _jumpRigidbodyComponent;
        
        private readonly HealthComponent _healthComponent;
        private readonly DeathRequestComponent _deathRequestComponent;
        
        private readonly GroundedComponent _groundedComponent;

        private readonly CharacterProvider _characterProvider;

        public Character(Rigidbody2D rigidbody, 
            MoveRequestComponent moveRequestComponent,
            MoveTransformComponent moveTransformComponent,
            LookComponent lookComponent, 
            JumpRequestComponent jumpRequestComponent,
            JumpRigidbodyComponent jumpRigidbodyComponent,
            HealthComponent healthComponent,
            DeathRequestComponent deathRequestComponent,
            GroundedComponent groundedComponent, 
            [Inject(Id = AttackType.Push)] AttackRequestComponent pushAttackRequestComponent, 
            [Inject(Id = AttackType.Toss)] AttackRequestComponent tossAttackRequestComponent, 
            [Inject(Id = AttackType.Push)] ForceAttackComponent pushAttackComponent, 
            [Inject(Id = AttackType.Toss)] ForceAttackComponent tossAttackComponent, 
            CharacterProvider characterProvider)
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
            _pushAttackRequestComponent = pushAttackRequestComponent;
            _tossAttackRequestComponent = tossAttackRequestComponent;
            _pushAttackComponent = pushAttackComponent;
            _tossAttackComponent = tossAttackComponent;
            _characterProvider = characterProvider;
        }
        
        public void Initialize()
        {
            LifeCycleBehaviourSetup();
            MovementBehaviourSetup();
            AttackBehaviorSetup();
            
            _healthComponent.OnDied += OnDied;
            
            _characterProvider.Register(_rigidbody.GetComponent<Entity>());
        }
        
        public void Dispose()
        {
            _healthComponent.OnDied -= OnDied;
            _characterProvider.Unregister();
        }

        public void Push() => _pushAttackRequestComponent.RequestAttack();
        
        public void Toss() => _tossAttackRequestComponent.RequestAttack();
        
        private void LifeCycleBehaviourSetup()
        {
            _deathRequestComponent.SetAction(this);
            _deathRequestComponent.SetCondition(this);
        }

        private void AttackBehaviorSetup()
        {
            PushAttack pushAttack = new(this);
            _pushAttackRequestComponent.SetCondition(pushAttack);
            _pushAttackRequestComponent.SetAction(pushAttack);
        
            TossAttack tossAttack = new(this);
            _tossAttackRequestComponent.SetCondition(tossAttack);
            _tossAttackRequestComponent.SetAction(tossAttack);
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
        
        private class PushAttack : AttackRequestComponent.IAction, AttackRequestComponent.ICondition
        {
            private readonly Character _parent;
        
            public PushAttack(Character parent) => _parent = parent;
            
            void AttackRequestComponent.IAction.Invoke()
                => _parent._pushAttackComponent.Attack();
        
            bool AttackRequestComponent.ICondition.Evaluate()
            {
                return _parent._tossAttackRequestComponent.IsRequested == false
                       && _parent._healthComponent.IsAlive;
            }
        }
        
        private class TossAttack : AttackRequestComponent.IAction, AttackRequestComponent.ICondition
        {
            private readonly Character _parent;
        
            public TossAttack(Character parent) => _parent = parent;

            void AttackRequestComponent.IAction.Invoke()
                => _parent._tossAttackComponent.Attack();
        
            bool AttackRequestComponent.ICondition.Evaluate()
            {
                return _parent._pushAttackRequestComponent.IsRequested == false
                       && _parent._healthComponent.IsAlive
                       && _parent._groundedComponent.IsGrounded;
            }
        }
    }
}