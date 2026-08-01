using System;
using Atomic.Elements;
using Atomic.Entities;
using Cysharp.Threading.Tasks;
using Game.GameEntity;
using UnityEngine;
using Event = Atomic.Elements.Event;

namespace Game.Weapon.Content.Hand
{
    public class HandWeaponInstaller : SceneEntityInstaller<IWeaponEntity>
    {
        private readonly DisposableComposite _disposables = new();
        private readonly Collider[] _colliders = new Collider[5];

        [SerializeField]
        private Cooldown _attackCooldown = 1;

        [SerializeField]
        private Cooldown _takeDamageDelay = 1.0333f;

        [SerializeField]
        private float _damage = 1f;

        [SerializeField]
        private float _attackRadius = 1f;

        private bool _inDelayProcess;

        public override void Install(IWeaponEntity weapon)
        {
            weapon.AddTag(WeaponEntityAPI.WeaponTag);
            weapon.AddValue(WeaponEntityAPI.Owner, new ReactiveVariable<IGameEntity>());

            weapon.AddValue(WeaponEntityAPI.FireCooldown, _attackCooldown);

            IEvent startAttackEvent = new Event();
            weapon.AddValue(WeaponEntityAPI.FireStartEvent, startAttackEvent);
            
            IRequest fireRequest = new Request();
            weapon.AddValue(WeaponEntityAPI.FireRequest, fireRequest);
            
            ICommand fireCommand = new Command();
            weapon.AddValue(WeaponEntityAPI.FireCommand, fireCommand);

            weapon.WhenFixedTick(_attackCooldown.Tick).AddTo(_disposables);
            
            weapon.WhenFixedTick(deltaTime =>
            {
                if (_inDelayProcess == false && fireRequest.Required && fireCommand.CanInvoke())
                {
                    Debug.Log($"Start timer {Time.time}");
                    _takeDamageDelay.ResetTime();
                    startAttackEvent.Invoke();
                    _inDelayProcess = true;
                }

                if (_inDelayProcess)
                    _takeDamageDelay.Tick(deltaTime);
                
                if (_takeDamageDelay.IsCompleted())
                {
                    _inDelayProcess = false;

                    if (fireRequest.Consume() && fireCommand.CanInvoke())
                    {
                        weapon.GetValue(WeaponEntityAPI.FireCommand).Invoke();
                    }
                }
            });
            
            SetupFireCommand(weapon);
        }

        public override void Uninstall(IWeaponEntity entity) => _disposables.Dispose();
        
        private void SetupFireCommand(IWeaponEntity weapon)
        {
            ICommand command = weapon.GetValue(WeaponEntityAPI.FireCommand);

            command.AddCondition(() =>
            {
                bool hasOwner = weapon.GetValue(WeaponEntityAPI.Owner).Value != null;
                bool isCooldownCompleted = weapon.GetValue(WeaponEntityAPI.FireCooldown).IsCompleted();

                return hasOwner && isCooldownCompleted;
            });

            command.AddAction(() =>
            {
                weapon.GetValue(WeaponEntityAPI.FireCooldown).ResetTime();

                IGameEntity owner = weapon.GetValue(WeaponEntityAPI.Owner).Value;
                Vector3 position = owner.GetValue(GameEntityAPI.Position).Value;

                int size = Physics.OverlapSphereNonAlloc(position, _attackRadius, _colliders);

                Debug.Log($"Colliders in buffer: {size}");

                if (size == 0)
                    return;

                for (int i = 0; i < size; i++)
                {
                    if (_colliders[i].TryGetComponent(out IGameEntity entity)
                        && entity.Equals(owner) == false)
                    {
                        if (entity.IsDead())
                            return;

                        entity.TryTakeDamage(_damage);
                    }
                }
            });
        }
    }
}