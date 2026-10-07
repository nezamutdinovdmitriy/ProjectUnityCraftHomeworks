using Game.Components;

namespace Game.UseCases
{
    public static class ManaUseCase
    {
        public static bool IsEmpty(this in Mana mana)
        {
            return mana.Value <= 0;
        }

        public static bool IsExpired(this in ManaRestoreDelay delay)
        {
            return delay.Time <= 0;
        }
    }
}