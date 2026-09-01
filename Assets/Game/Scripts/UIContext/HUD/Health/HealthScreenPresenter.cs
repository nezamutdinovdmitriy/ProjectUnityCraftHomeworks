using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities;

namespace Game.UI
{
    public class HealthScreenPresenter : IUIContextInit, IUIContextDispose
    {
        private readonly DisposableComposite _disposables = new();
        private readonly GameContext _gameContext;

        private HealthScreenView _view;
        private IGameEntity _entity;
        
        public HealthScreenPresenter(GameContext gameContext) 
            => _gameContext = gameContext;

        public void Init(IUIContext context)
        {
            _view = context.GetValue(UIContextAPI.HealthScreenView);
            _entity = _gameContext.GetValue(GameContextAPI.Character).Value;
            
            _entity.GetValue(GameEntityAPI.CurrentHealth).Observe(OnHealthChanged).AddTo(_disposables);
        }

        public void Dispose(IUIContext context) 
            => _disposables?.Dispose();

        private void OnHealthChanged(float health)
        {
            float percent = health / _entity.GetValue(GameEntityAPI.MaxHealth).Value;
            _view.ChangePercent(percent);
            _view.TakeDamage((int) health);
        }
    }
}