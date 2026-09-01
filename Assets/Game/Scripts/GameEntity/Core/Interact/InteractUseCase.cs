using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntities.Weapon;

namespace Game.GameEntities
{
    public static class InteractUseCase
    {
        public static void Interact(this IGameEntity interactor, IGameEntity interactable)
        {
            bool isInteractable = interactable.HasTag(GameEntityAPI.InteractableTag);

            if (isInteractable == false)
                return;
            
            interactable.GetValue(GameEntityAPI.InteractCommand).Invoke(interactor);
        }
        
        public static void PickupAmmo(this IGameEntity entity, int value)
        {
            IReactiveVariable<IWeaponEntity> weapon = entity.GetValue(GameEntityAPI.Weapon);
            IReactiveVariable<int> ammo = weapon.Value.GetValue(WeaponEntityAPI.Ammo);
            
            ammo.Value += value;
        }
    }
}