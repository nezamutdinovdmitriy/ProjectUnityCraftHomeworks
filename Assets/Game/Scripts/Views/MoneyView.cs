using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Views
{
    public class MoneyView : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private TMP_Text _value;

        [SerializeField]
        private RectTransform _scaleTarget;

        [Header("Animation Settings")] [SerializeField]
        private float _scrollDuration = 0.3f;

        [SerializeField]
        private float _earnDelay = 1f;

        [SerializeField]
        private float _scalePunchIntensity = 0.15f;

        [SerializeField]
        private float _scaleDuration = 0.2f;

        private int _animatedMoneyValue;
        private Tween _counterTween;
        private Tween _delayTween;

        private void OnDestroy()
        {
            KillAllTweens();
            _scaleTarget?.DOKill();
        }
        
        public void SetText(string value)
        {
            KillAllTweens();

            int.TryParse(value, out _animatedMoneyValue);

            _value.text = value;
            ResetScale();
        }

        public void SetTextAnimatedWithDelay(int targetValue)
        {
            _delayTween?.Kill();

            _delayTween = DOVirtual.DelayedCall(_earnDelay, () =>
            {
                StartScrollTween(targetValue);
                _delayTween = null;
            }, false).SetLink(gameObject);
        }

        public void SetTextAnimated(int targetValue)
        {
            if (_delayTween.IsActive() && _delayTween.IsPlaying())
                return;

            StartScrollTween(targetValue);
        }

        private void StartScrollTween(int targetValue)
        {
            _counterTween?.Kill();

            _counterTween = DOTween.To(() => _animatedMoneyValue, x =>
                {
                    _animatedMoneyValue = x;
                    _value.text = _animatedMoneyValue.ToString();
                }, targetValue, _scrollDuration)
                .SetEase(Ease.Linear)
                .SetLink(gameObject);

            PlayPunchAnimation();
        }

        private void PlayPunchAnimation()
        {
            if (_scaleTarget == null) return;

            _scaleTarget.DOKill();
            _scaleTarget.localScale = Vector3.one;
            _scaleTarget.DOPunchScale(
                    new Vector3(_scalePunchIntensity, _scalePunchIntensity, 0f),
                    _scaleDuration, 1, 0.5f)
                .SetLink(gameObject);
        }

        private void ResetScale()
        {
            if (_scaleTarget != null)
            {
                _scaleTarget.DOKill();
                _scaleTarget.localScale = Vector3.one;
            }
        }

        private void KillAllTweens()
        {
            _delayTween?.Kill();
            _delayTween = null;

            _counterTween?.Kill();
            _counterTween = null;
        }
    }
}