using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity;

namespace Game.UI
{
    public class HealthScreenPresenter : IUIContextInit, IUIContextDispose
    {
        private readonly DisposableComposite _disposables = new();
        private readonly IGameEntity _entity;
        
        private HealthScreenView _view;
        
        public HealthScreenPresenter(IGameEntity entity) 
            => _entity = entity;

        public void Init(IUIContext entity)
        {
            _view = entity.GetValue(UIContextAPI.HealthScreenView);
            _entity.GetValue(GameEntityAPI.CurrentHealth).Observe(OnHealthChanged).AddTo(_disposables);
        }

        public void Dispose(IUIContext entity) 
            => _disposables?.Dispose();

        private void OnHealthChanged(float health)
        {
            float percent = health / _entity.GetValue(GameEntityAPI.MaxHealth).Value;
            _view.ChangePercent(percent);
            _view.TakeDamage((int) health);
        }
    }
}