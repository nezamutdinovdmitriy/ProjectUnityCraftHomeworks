using Atomic.Elements;
using Atomic.Entities;

namespace Game.UI
{
    public class AmmoViewPresenter : IUIContextInit, IUIContextDispose
    {
        private readonly DisposableComposite _disposables = new();
        private readonly IReactiveVariable<int> _ammo;
        
        private StatView _view;

        public AmmoViewPresenter(IReactiveVariable<int> ammo) 
            => _ammo = ammo;

        public void Init(IUIContext entity)
        {
            _view = entity.GetValue(UIContextAPI.AmmoView);
            _ammo.Observe(OnAmmoChanged).AddTo(_disposables);
        }

        public void Dispose(IUIContext entity) => _disposables?.Dispose();

        private void OnAmmoChanged(int amount) => _view.SetText(_ammo.ToString());
    }
}