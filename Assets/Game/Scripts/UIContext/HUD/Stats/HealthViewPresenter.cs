using System.Globalization;
using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity;

namespace Game.UI
{
    public class HealthViewPresenter : IUIContextInit, IUIContextDispose
    {
        private readonly DisposableComposite _disposables = new();
        
        private readonly IReactiveVariable<float> _currentHealth;
        private readonly IValue<float> _maxHealth;

        private StatView _view;

        public HealthViewPresenter(IGameEntity entity)
        {
            _maxHealth = entity.GetValue(GameEntityAPI.MaxHealth);
            _currentHealth = entity.GetValue(GameEntityAPI.CurrentHealth);
        }

        public void Init(IUIContext entity)
        {
            _view = entity.GetValue(UIContextAPI.HealthView);
            _currentHealth.Observe(OnHealthChanged).AddTo(_disposables);
        }

        public void Dispose(IUIContext entity) => _disposables?.Dispose();

        private void OnHealthChanged(float health)
        {
            _view.SetText(health.ToString(CultureInfo.InvariantCulture));
            _view.SetProgress(health / _maxHealth.Value);
        }
    }
}