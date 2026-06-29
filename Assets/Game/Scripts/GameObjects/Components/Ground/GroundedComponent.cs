using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Zenject;

namespace Game
{
    [Serializable]
    public sealed class GroundedComponent : IFixedTickable
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField]
            public Transform Feet { get; private set; }

            [field: SerializeField]
            public LayerMask LayerMask { get; private set; }
        
            [field: SerializeField]
            public float GroundDistance { get; private set; } = 0.15f;
        }
        
        public event Action<bool> OnGrounded;

        private Settings _settings;
        
        [ShowInInspector, ReadOnly]
        private Transform _ground;
        
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private bool _isGrounded;

        public GroundedComponent(Settings settings) => _settings = settings;
        
        public Transform Ground => _ground;
        
        public bool IsGrounded => _isGrounded;
        
        public void FixedTick()
        {
            RaycastHit2D hit = Physics2D.Raycast(
                _settings.Feet.position, 
                Vector2.down, 
                _settings.GroundDistance, 
                _settings.LayerMask);

            bool grounded = hit;
            
            if (grounded != _isGrounded)
            {
                _isGrounded = grounded;
                _ground = _isGrounded ? hit.transform : null;
                this.OnGrounded?.Invoke(_isGrounded);
            }
            else
            {
                _ground = _isGrounded ? hit.transform : null;
            }
        }
    }
}