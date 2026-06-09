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

        private IMoneyStorage _moneyStorage;
        
        [Inject]
        public void Construct(IMoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
        }

        public void Initialize()
        {
            _moneyStorage.OnMoneySpent += OnMoneySpent;
            _moneyStorage.OnMoneyChanged += OnMoneyChanged;
            _moneyStorage.OnMoneyEarned += OnMoneyEarned;
            
            _view.RenderInstant(_moneyStorage.Money.ToString());
        }
        
        private void OnDestroy()
        {
            _moneyStorage.OnMoneySpent -= OnMoneySpent;
            _moneyStorage.OnMoneyChanged -= OnMoneyChanged;
            _moneyStorage.OnMoneyEarned -= OnMoneyEarned;
        }

        private void OnMoneyEarned(int newValue, int range) => _view.RenderEarned(newValue);
        private void OnMoneyChanged(int newValue, int prevValue) => _view.RenderSmooth(newValue);
        private void OnMoneySpent(int newValue, int range) => _view.RenderInstant(newValue.ToString());
    }
}