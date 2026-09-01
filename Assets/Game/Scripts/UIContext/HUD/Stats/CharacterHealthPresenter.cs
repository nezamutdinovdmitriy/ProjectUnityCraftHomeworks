using System.Globalization;
using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities;

namespace Game.UI
{
    public class CharacterHealthPresenter : IUIContextInit, IUIContextDispose
    {
        private readonly DisposableComposite _disposables = new();
        private readonly GameContext _gameContext;
        
        private StatView _view;
        private IValue<float> _maxHealth;

        public CharacterHealthPresenter(GameContext gameContext) 
            => _gameContext = gameContext;

        public void Init(IUIContext context)
        {
            _view = context.GetValue(UIContextAPI.HealthView);
            IGameEntity entity = _gameContext.GetValue(GameContextAPI.Character).Value;

            _maxHealth = entity.GetValue(GameEntityAPI.MaxHealth);
            entity.GetValue(GameEntityAPI.CurrentHealth).Observe(OnHealthChanged).AddTo(_disposables);
        }

        public void Dispose(IUIContext context) => _disposables?.Dispose();

        private void OnHealthChanged(float currentHealth)
        {
            _view.SetText(currentHealth.ToString(CultureInfo.InvariantCulture));
            _view.SetProgress(currentHealth / _maxHealth.Value);
        }
    }
}