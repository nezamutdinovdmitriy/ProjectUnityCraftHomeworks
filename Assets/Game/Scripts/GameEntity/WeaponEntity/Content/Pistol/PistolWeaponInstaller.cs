using Atomic.Elements;
using Atomic.Entities;
using Game.Bullets;
using Game.GameEntity;
using Game.Weapon.Content;
using UnityEngine;

namespace Game.Weapon
{
    public class PistolWeaponInstaller : SceneEntityInstaller<IWeaponEntity>
    {
        private readonly DisposableComposite _disposables = new();

        [SerializeField]
        private Cooldown _fireCooldown;

        [SerializeField]
        private int _initialAmmoAmount;

        [SerializeField]
        private Transform _firePoint;

        public override void Install(IWeaponEntity weapon)
        {
            IGameContext gameContext = GameContext.Instance;

            weapon.AddTag(WeaponEntityAPI.WeaponTag);

            weapon.AddValue(WeaponEntityAPI.Owner, new ReactiveVariable<IGameEntity>());

            weapon.AddValue(WeaponEntityAPI.Ammo, new ReactiveVariable<int>(_initialAmmoAmount));

            weapon.AddValue(WeaponEntityAPI.FireRequest, new Request());
            weapon.AddValue(WeaponEntityAPI.FireCommand, new Command());

            weapon.AddValue(WeaponEntityAPI.FireCooldown, _fireCooldown);
            weapon.WhenFixedTick(_fireCooldown.Tick).AddTo(_disposables);

            SetupFireCommand(weapon, gameContext);
            
            weapon.AddBehaviour(new PistolFireBehaviour());
        }

        public override void Uninstall(IWeaponEntity entity) => _disposables.Dispose();

        private void SetupFireCommand(IWeaponEntity weapon, IGameContext gameContext)
        {
            ICommand command = weapon.GetValue(WeaponEntityAPI.FireCommand);

            command.AddCondition(() => weapon.HasOwner() && weapon.HasAmmo() && weapon.IsFireCooldownCompleted());
            command.AddAction(() => weapon.Fire(gameContext, _firePoint));
        }
    }
}