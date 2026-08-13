using System;
using UnityEngine;

namespace GameObjects.Components
{
    public sealed class CollisionComponent : MonoBehaviour
    {
        public event Action<Collision2D> OnEntered;
        public event Action<Collision2D> OnExited;

        private void OnCollisionEnter2D(Collision2D other) => this.OnEntered?.Invoke(other);

        private void OnCollisionExit2D(Collision2D other) => this.OnExited?.Invoke(other);
    }
}