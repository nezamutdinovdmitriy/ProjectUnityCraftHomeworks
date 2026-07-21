using Atomic.Elements;
using Atomic.Entities;

namespace Game.EntityContext.Core.Interact
{
    public static class InteractUseCase
    {
        public static void Interact(this IEntityContext entity, IEntityContext interactable)
        {
            ICommand<IEntityContext> command = interactable.GetValue(EntityContextAPI.InteractCommand);

            if (interactable.HasTag(EntityContextAPI.InteractableTag)
                && command.CanInvoke(entity))
            {
                command.Invoke(entity);
            }
        }
    }
}