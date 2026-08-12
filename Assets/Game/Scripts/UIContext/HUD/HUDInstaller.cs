using Atomic.Entities;
using Game.GameEntity;
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
            IGameEntity character = gameContext.GetValue(GameContextAPI.Character).Value;

            entity.AddBehaviour(new HealthScreenPresenter(character));
            entity.AddBehaviour(new CharacterHealthPresenter(character));
            entity.AddBehaviour(new CharacterAmmoPresenter(character));
            entity.AddBehaviour(new CharacterKillsPresenter(character));
            
            entity.AddBehaviour(new CharacterMovementController());
            entity.AddBehaviour(new CharacterAimController());
            entity.AddBehaviour(new CharacterRotateController());
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