namespace Game.Components
{
    public static class LifetimeUseCase
    {
        public static bool IsExpired(this in Lifetime lifetime)
        {
            return lifetime.Value <= 0;
        }
    }
}