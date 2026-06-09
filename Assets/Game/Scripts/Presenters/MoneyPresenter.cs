using DG.Tweening;
using Game.Views;
using Modules.Money;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Presenters
{
    public class MoneyPresenter : MonoBehaviour, IInitializable
    {
        [SerializeField]
        private MoneyView _view;

        [SerializeField]
        private float _scrollDuration = 0.3f;
        
        private int _visualMoney;
        private Tween _counterTween;
        private Tween _delayTween;

        private IMoneyStorage _moneyStorage;
        
        [Inject]
        public void Construct(IMoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;

            _visualMoney = _moneyStorage.Money;
            _view.SetValue(_visualMoney.ToString());
        }

        public void Initialize()
        {
            _moneyStorage.OnMoneySpent += OnMoneySpent;
            _moneyStorage.OnMoneyChanged += OnMoneyChanged;
            _moneyStorage.OnMoneyEarned += OnMoneyEarned;
        }
        
        private void OnDestroy()
        {
            _moneyStorage.OnMoneySpent -= OnMoneySpent;
            _moneyStorage.OnMoneyChanged -= OnMoneyChanged;
            _moneyStorage.OnMoneyEarned -= OnMoneyEarned;

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
                .SetEase(Ease.Linear)
                .SetLink(gameObject); // Строго линейно!

            _view.PlayAnimation();
        }

        private void OnMoneyEarned(int newValue, int range)
        {
            int targetMoney = newValue;
            
            if (_delayTween.IsActive()) 
                _delayTween.Kill();

            _delayTween = DOVirtual.DelayedCall(1f, () =>
            {
                AnimateToTarget(targetMoney);
                _delayTween = null;
            }, false).SetLink(gameObject);
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
            if (_delayTween.IsActive()) 
                _delayTween.Kill();
            
            _delayTween = null;

            if (_counterTween.IsActive())
                _counterTween.Kill();
            
            _counterTween = null;

            _visualMoney = targetValue;
            _view.SetValue(_visualMoney.ToString());
            
            _view.ResetAnimation();
        }
    }
}