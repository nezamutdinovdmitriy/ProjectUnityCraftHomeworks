using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities;
using Game.GameEntities.Weapon;

namespace Game.UI
{
    public class CharacterAmmoPresenter : IUIContextInit, IUIContextDispose
    {
        private readonly DisposableComposite _disposables = new();
        private readonly GameContext _gameContext;
        
        private StatView _view;

        public CharacterAmmoPresenter(GameContext gameContext) 
            => _gameContext = gameContext;

        public void Init(IUIContext context)
        {
            _view = context.GetValue(UIContextAPI.AmmoView);
            IGameEntity entity = _gameContext.GetValue(GameContextAPI.Character).Value;
            
            IReactiveVariable<IWeaponEntity> weapon = entity.GetValue(GameEntityAPI.Weapon);
            weapon.Value.GetValue(WeaponEntityAPI.Ammo).Observe(OnAmmoChanged).AddTo(_disposables);
        }

        public void Dispose(IUIContext context) => _disposables?.Dispose();

        private void OnAmmoChanged(int amount) => _view.SetText(amount.ToString());
    }
}