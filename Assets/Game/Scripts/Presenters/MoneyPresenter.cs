using DG.Tweening;
using Game.Views;
using Modules.Money;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Presenters
{
    public class MoneyPresenter : MonoBehaviour
    {
        [SerializeField]
        private MoneyView _view;

        [SerializeField]
        private float _scrollDuration = 0.3f;

        [SerializeField]
        private RectTransform _scaleTarget;

        [SerializeField]
        private float _scalePunchIntensity = 0.15f;

        [SerializeField]
        private float _scaleDuration = 0.2f;

        private IMoneyStorage _moneyStorage;
        private int _visualMoney;
        private Tween _counterTween;
        private Tween _delayTween;

        [Inject]
        public void Construct(IMoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;

            _visualMoney = _moneyStorage.Money;
            _view.SetValue(_visualMoney.ToString());

            _moneyStorage.OnMoneySpent += OnMoneySpent;
            _moneyStorage.OnMoneyChanged += OnMoneyChanged;
            _moneyStorage.OnMoneyEarned += OnMoneyEarned;
        }

        private void OnDestroy()
        {
            if (_moneyStorage != null)
            {
                _moneyStorage.OnMoneySpent -= OnMoneySpent;
                _moneyStorage.OnMoneyChanged -= OnMoneyChanged;
                _moneyStorage.OnMoneyEarned -= OnMoneyEarned;
            }

            _delayTween?.Kill();
            _counterTween?.Kill();
        }

        public void AnimateToTarget(int amount)
        {
            _counterTween?.Kill();
            _counterTween = DOTween.To(() => _visualMoney, x =>
                {
                    _visualMoney = x;
                    _view.SetValue(_visualMoney.ToString());
                }, amount, _scrollDuration)
                .SetEase(Ease.Linear); // Строго линейно!

            if (_scaleTarget != null)
            {
                _scaleTarget.DOKill();
                _scaleTarget.localScale = Vector3.one;
                _scaleTarget.DOPunchScale(
                    new Vector3(
                        _scalePunchIntensity,
                        _scalePunchIntensity,
                        0f),
                    _scaleDuration, 1, 0.5f
                );
            }
        }

        private void OnMoneyEarned(int newValue, int range)
        {
            int targetMoney = newValue;
            
            if (_delayTween.IsActive()) 
                _delayTween.Kill();

            _delayTween = DOVirtual.DelayedCall(1f, () =>
            {
                AnimateToTarget(targetMoney);
                _delayTween = null; // Обязательно чистим ссылку по завершению!
            }, false);
        }

        private void OnMoneyChanged(int newValue, int prevValue)
        {
            if (_delayTween.IsActive() && _delayTween.IsPlaying()) 
                return;

            ResetVisuals(newValue);
        }

        private void OnMoneySpent(int newValue, int range) => ResetVisuals(newValue);

        private void ResetVisuals(int targetValue)
        {
            if (_delayTween.IsActive()) _delayTween.Kill();
            _delayTween = null;

            if (_counterTween.IsActive()) _counterTween.Kill();
            _counterTween = null;

            if (_scaleTarget != null) _scaleTarget.DOKill();
            
            _visualMoney = targetValue;
            _view.SetValue(_visualMoney.ToString());
            if (_scaleTarget != null) _scaleTarget.localScale = Vector3.one;
        }
    }
}