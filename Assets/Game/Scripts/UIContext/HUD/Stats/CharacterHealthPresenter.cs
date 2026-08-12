using System.Globalization;
using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity;

namespace Game.UI
{
    public class CharacterHealthPresenter : IUIContextInit, IUIContextDispose
    {
        private readonly DisposableComposite _disposables = new();
        private readonly IGameEntity _character;
        
        private StatView _view;

        private IValue<float> _maxHealth;

        public CharacterHealthPresenter(IGameEntity character) 
            => _character = character;

        public void Init(IUIContext entity)
        {
            _view = entity.GetValue(UIContextAPI.HealthView);

            _maxHealth = _character.GetValue(GameEntityAPI.MaxHealth);
            _character.GetValue(GameEntityAPI.CurrentHealth).Observe(OnHealthChanged).AddTo(_disposables);
        }

        public void Dispose(IUIContext entity) => _disposables?.Dispose();

        private void OnHealthChanged(float currentHealth)
        {
            _view.SetText(currentHealth.ToString(CultureInfo.InvariantCulture));
            _view.SetProgress(currentHealth / _maxHealth.Value);
        }
    }
}