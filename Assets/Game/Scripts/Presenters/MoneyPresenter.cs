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

        private IMoneyStorage _moneyStorage;
        
        [Inject]
        public void Construct(IMoneyStorage moneyStorage) => _moneyStorage = moneyStorage;

        private void Start() => _view.SetText(_moneyStorage.Money.ToString());

        public void OnEnable()
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
        }

        private void OnMoneyEarned(int newValue, int range) => _view.SetTextAnimatedWithDelay(newValue);
        private void OnMoneyChanged(int newValue, int prevValue) => _view.SetTextAnimated(newValue);
        private void OnMoneySpent(int newValue, int range) => _view.SetText(newValue.ToString());
    }
}