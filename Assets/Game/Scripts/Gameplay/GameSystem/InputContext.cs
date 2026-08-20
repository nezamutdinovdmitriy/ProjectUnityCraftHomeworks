using UnityEngine;

namespace SampleGame
{
    public struct InputContext
    {
        public bool leftClick;
        public bool rightClick;
        public Vector3 mousePosition;
        
        public Vector3? point;
        public GameObject target;
        public bool enqueueCommand;
    }
}