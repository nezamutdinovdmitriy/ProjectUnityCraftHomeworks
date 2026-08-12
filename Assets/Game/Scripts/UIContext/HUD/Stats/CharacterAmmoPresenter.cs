using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities;
using Game.GameEntities.Weapon;

namespace Game.UI
{
    public class CharacterAmmoPresenter : IUIContextInit, IUIContextDispose
    {
        private readonly DisposableComposite _disposables = new();
        private readonly IGameEntity _character;
        
        private StatView _view;

        public CharacterAmmoPresenter(IGameEntity character) 
            => _character = character;

        public void Init(IUIContext entity)
        {
            _view = entity.GetValue(UIContextAPI.AmmoView);

            IReactiveVariable<IWeaponEntity> weapon = _character.GetValue(GameEntityAPI.Weapon);
            weapon.Value.GetValue(WeaponEntityAPI.Ammo).Observe(OnAmmoChanged).AddTo(_disposables);
        }

        public void Dispose(IUIContext entity) => _disposables?.Dispose();

        private void OnAmmoChanged(int amount) => _view.SetText(amount.ToString());
    }
}