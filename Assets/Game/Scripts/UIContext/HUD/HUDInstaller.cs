using Atomic.Entities;
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
        private StatView _healthView;
        
        [SerializeField]
        private StatView _ammoView;

        [SerializeField]
        private StatView _killsView;
        
        public override void Install(IUIContext entity)
        {
            GameContext gameContext = GameContext.Instance;

            _joystickInstaller.Install(entity);
            
            AddValues(entity);
            AddBehaviours(entity, gameContext);
        }

        private void AddBehaviours(IUIContext entity, GameContext gameContext)
        {
            entity.AddBehaviour(new HealthScreenPresenter(gameContext));
            entity.AddBehaviour(new CharacterHealthPresenter(gameContext));
            entity.AddBehaviour(new CharacterAmmoPresenter(gameContext));
            entity.AddBehaviour(new CharacterScorePresenter(gameContext));
            
            entity.AddBehaviour(new CharacterMovementController(gameContext));
            entity.AddBehaviour(new CharacterAimController(gameContext));
        }

        private void AddValues(IUIContext entity)
        {
            entity.AddValue(UIContextAPI.HealthScreenView, _healthScreenView);
            entity.AddValue(UIContextAPI.HealthView, _healthView);
            entity.AddValue(UIContextAPI.AmmoView, _ammoView);
            entity.AddValue(UIContextAPI.KillsView, _killsView);
        }
    }
}