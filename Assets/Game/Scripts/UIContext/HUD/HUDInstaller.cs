using Atomic.Entities;
using UnityEngine;

namespace Game.UI
{
    public class HUDInstaller : SceneEntityInstaller<IUIContext>
    {
        [SerializeField]
        private JoystickInstaller _joystickInstaller;
        
        public override void Install(IUIContext entity)
        {
            _joystickInstaller.Install(entity);
        }
    }
}