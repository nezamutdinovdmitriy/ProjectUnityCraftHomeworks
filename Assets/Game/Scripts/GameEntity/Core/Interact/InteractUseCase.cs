using Game.GameEntity;

namespace Atomic.Entities
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
    }
}