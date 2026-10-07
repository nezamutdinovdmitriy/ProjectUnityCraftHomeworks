using Game.Components;

namespace Game.UseCases
{
    public static class LifetimeUseCase
    {
        public static bool IsExpired(this in Lifetime lifetime)
        {
            return lifetime.Value <= 0;
        }
    }
}