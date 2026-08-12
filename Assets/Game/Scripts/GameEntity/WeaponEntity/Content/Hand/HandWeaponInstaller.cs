using Atomic.Elements;
using Atomic.Entities;
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
        private Transform _firePoint;
        
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
            
            SetupFireCommand(weapon);
            
            weapon.AddBehaviour(new HandAttackBehaviour(_takeDamageDelay));
        }

        public override void Uninstall(IWeaponEntity entity) => _disposables.Dispose();
        
        private void SetupFireCommand(IWeaponEntity weapon)
        {
            ICommand command = weapon.GetValue(WeaponEntityAPI.FireCommand);

            command.AddCondition(() =>
            {
                bool hasOwner = weapon.GetValue(WeaponEntityAPI.Owner).Value != null;
                bool isOwnerAlive = weapon.GetValue(WeaponEntityAPI.Owner).Value.IsDead() == false;
                bool isCooldownCompleted = weapon.GetValue(WeaponEntityAPI.FireCooldown).IsCompleted();

                return hasOwner && isOwnerAlive && isCooldownCompleted;
            });

            command.AddAction(() =>
            {
                weapon.GetValue(WeaponEntityAPI.FireCooldown).ResetTime();

                IGameEntity owner = weapon.GetValue(WeaponEntityAPI.Owner).Value;

                int size = Physics.OverlapSphereNonAlloc(_firePoint.position, _attackRadius, _colliders);

                if (size == 0)
                    return;

                for (int i = 0; i < size; i++)
                {
                    if (_colliders[i].TryGetComponent(out IGameEntity entity)
                        && entity.Equals(owner) == false
                        && entity.HasTag(GameEntityAPI.CharacterTag))
                    {
                        if (entity.IsDead())
                            return;

                        entity.TryInvokeTakeDamageCommand(_damage);
                        return;
                    }
                }
            });
        }
    }
}