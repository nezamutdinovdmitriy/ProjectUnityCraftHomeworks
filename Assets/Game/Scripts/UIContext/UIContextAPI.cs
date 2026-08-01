using Atomic.Entities;

namespace Game.UI
{
    public static class UIContextAPI
    {
        public static ValueKey<IUIContext, Joystick> MovementJoystick = new(nameof(MovementJoystick));
        public static ValueKey<IUIContext, Joystick> AimJoystick = new(nameof(AimJoystick));

        public static ValueKey<IUIContext, HealthScreenView> HealthScreenView = new(nameof(HealthScreenView));
        
        public static ValueKey<IUIContext, StatView> HealthView = new(nameof(HealthView));
        public static ValueKey<IUIContext, StatView> AmmoView = new(nameof(AmmoView));
    }
}