using UnityEngine;

namespace Game.Scripts
{
    public sealed class ApplicationRate : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}