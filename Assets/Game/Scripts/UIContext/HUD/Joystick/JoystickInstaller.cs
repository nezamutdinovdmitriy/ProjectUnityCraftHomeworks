using System;
using Atomic.Entities;
using UnityEngine;

namespace Game.UI
{
    [Serializable]
    public class JoystickInstaller : IEntityInstaller<IUIContext>
    {
        [SerializeField]
        private Joystick _movementJoystick;

        [SerializeField]
        private Joystick _aimJoystick;
        
        public void Install(IUIContext entity)
        {
            entity.AddValue(UIContextAPI.MovementJoystick, _movementJoystick);
            entity.AddValue(UIContextAPI.AimJoystick, _aimJoystick);
        }
    }
}