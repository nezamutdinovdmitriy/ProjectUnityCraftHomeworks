using DG.Tweening;
using UnityEngine;

namespace Game
{
    public sealed class TakeDamageColorComponent : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer[] _renderers;

        [SerializeField]
        private Color _damagedColor = Color.red;

        [SerializeField]
        private float _frequency = 0.2f;

        [SerializeField]
        private int _loops = 3;
        
        private Color _baseColor;

        private float _blend; // 0..1
        private Tween _tween;

        private void Reset() => _renderers = this.GetComponentsInChildren<SpriteRenderer>();

        private void Awake() => _baseColor = _renderers is {Length: > 0} ? _renderers[0].color : Color.white;

        public void TakeDamage()
        {
            _tween?.Kill();

            _tween = DOTween.Sequence()
                .Append(DOTween.To(() => _blend, x => _blend = x, 1f, _frequency))
                .Append(DOTween.To(() => _blend, x => _blend = x, 0f, _frequency))
                .SetLoops(_loops, LoopType.Restart);
        }

        private void LateUpdate()
        {
            Color color = Color.Lerp(_baseColor, _damagedColor, _blend);

            foreach (var r in _renderers)
                r.color = new Color(color.r, color.g, color.b, r.color.a);;
        }

        private void OnDisable() => _tween?.Kill();
    }
}