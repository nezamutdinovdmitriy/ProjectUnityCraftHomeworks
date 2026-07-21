using Atomic.Entities;
using UnityEngine;

namespace Game.UI
{
    public class JoystickInstaller : SceneEntityInstaller<UIContext>
    {
        [SerializeField]
        private Joystick _movementJoystick;
        
        [SerializeField]
        private Joystick _fireJoystick;
        
        public override void Install(UIContext entity)
        {
            entity.AddValue(UIContextAPI.MovementJoystick, _movementJoystick);
            entity.AddValue(UIContextAPI.FireJoystick, _fireJoystick);
        }
    }
}