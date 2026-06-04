using Modules;
using UnityEngine;

namespace GameSystems
{
    public class KeyboardInputProvider : IInputProvider
    {
        public SnakeDirection GetDirection()
        {
            if (Input.GetKeyDown(KeyCode.W)
                || Input.GetKeyDown(KeyCode.UpArrow))
                return SnakeDirection.UP;

            if (Input.GetKeyDown(KeyCode.S)
                || Input.GetKeyDown(KeyCode.DownArrow))
                return SnakeDirection.DOWN;

            if (Input.GetKeyDown(KeyCode.A)
                || Input.GetKeyDown(KeyCode.LeftArrow))
                return SnakeDirection.LEFT;

            if (Input.GetKeyDown(KeyCode.D)
                || Input.GetKeyDown(KeyCode.RightArrow))
                return SnakeDirection.RIGHT;

            return SnakeDirection.NONE;
        }
    }
}