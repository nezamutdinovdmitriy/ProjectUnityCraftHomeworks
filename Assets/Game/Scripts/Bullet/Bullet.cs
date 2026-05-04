using System;
using UnityEngine;

namespace Game
{
    // +
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet, Collider2D> TriggerEntered;

        public TeamType team = TeamType.None;
        public Vector2 direction;

        public int damage;
        public float speed;
        public GameObject blueVFX;
        public GameObject redVFX;

        private void OnTriggerEnter2D(Collider2D other) => TriggerEntered?.Invoke(this, other);
    }
}