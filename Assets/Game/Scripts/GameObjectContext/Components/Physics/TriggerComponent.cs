using System;
using UnityEngine;

namespace GameObjects.Components
{
    public sealed class TriggerComponent : MonoBehaviour
    {
        public event Action<Collider2D> OnEntered;
        public event Action<Collider2D> OnExited;

        private void OnTriggerEnter2D(Collider2D col) => this.OnEntered?.Invoke(col);

        private void OnTriggerExit2D(Collider2D col) => this.OnExited?.Invoke(col);
    }
}