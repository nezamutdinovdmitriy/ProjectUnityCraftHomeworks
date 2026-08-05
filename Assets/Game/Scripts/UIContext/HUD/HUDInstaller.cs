using Atomic.Entities;
using Game.GameEntity;
using Game.Weapon;
using UnityEngine;

namespace Game.UI
{
    public class HUDInstaller : SceneEntityInstaller<IUIContext>
    {
        [SerializeField]
        private JoystickInstaller _joystickInstaller;

        [SerializeField]
        private HealthScreenView _healthScreenView;

        [SerializeField]
        private GameEntity.GameEntity _character;

        [SerializeField]
        private StatView _healthView;
        
        [SerializeField]
        private StatView _ammoView;

        [SerializeField]
        private StatView _killsView;
        
        public override void Install(IUIContext entity)
        {
            entity.AddValue(UIContextAPI.HealthScreenView, _healthScreenView);
            entity.AddValue(UIContextAPI.HealthView, _healthView);
            entity.AddValue(UIContextAPI.AmmoView, _ammoView);
            entity.AddValue(UIContextAPI.KillsView, _killsView);
            
            _joystickInstaller.Install(entity);
            
            entity.AddBehaviour(new HealthScreenPresenter(_character));
            entity.AddBehaviour(new HealthViewPresenter(_character));
            entity.AddBehaviour(new AmmoViewPresenter(
                _character.GetValue(GameEntityAPI.Weapon).Value
                    .GetValue(WeaponEntityAPI.Ammo)));
            entity.AddBehaviour(new KillsViewPresenter(
                _character.GetValue(GameEntityAPI.Score)));
        }
    }
}