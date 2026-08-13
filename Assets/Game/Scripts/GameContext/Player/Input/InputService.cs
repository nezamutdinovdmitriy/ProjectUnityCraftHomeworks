using UnityEngine;
using UnityEngine.UIElements;

namespace GameContexts
{
    public class InputService
    {
        private const string HorizontalAxisName = "Horizontal";
        
        public bool IsEnabled { get; set; } = true;

        public bool IsJumped
        {
            get
            {
                if (IsEnabled == false)
                    return false;

                return Input.GetKeyDown(KeyCode.Space);
            }
        }

        public Vector2 MoveDirection
        {
            get
            {
                if (IsEnabled == false)
                    return Vector2.zero;

                return new Vector2(Input.GetAxisRaw(HorizontalAxisName), 0);
            }
        }

        public bool IsMainAttacked
        {
            get
            {
                if (IsEnabled == false)
                    return false;

                return Input.GetMouseButtonDown((int) MouseButton.LeftMouse);
            }
        }
        
        public bool IsAdditionalAttacked
        {
            get
            {
                if (IsEnabled == false)
                    return false;

                return Input.GetMouseButtonDown((int) MouseButton.RightMouse);
            }
        }
        
    }
}