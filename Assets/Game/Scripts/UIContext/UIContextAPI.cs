using Atomic.Entities;

namespace Game.UIContext
{
    public static class UIContextAPI
    {
        public static ValueKey<IUIContext, Joystick> MovementJoystick = new(nameof(MovementJoystick));
        public static ValueKey<IUIContext, Joystick> AimJoystick = new(nameof(AimJoystick));
    }
}