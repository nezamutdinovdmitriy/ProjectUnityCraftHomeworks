using System;
using Modules.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlanetView : MonoBehaviour
    {
        public event Action PlanetButtonClicked;
        public event Action PlanetButtonHeld;

        [SerializeField]
        private SmartButton _button;
        
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Image _lock;

        [SerializeField]
        private Image _coin;

        [Header("Income")] [SerializeField]
        private GameObject _incomeRoot;

        [SerializeField]
        private Image _incomeProgress;

        [SerializeField]
        private TMP_Text _incomeTimeText;

        [Header("Price")] [SerializeField]
        private GameObject _priceRoot;

        [SerializeField]
        private Image _priceIcon;

        [SerializeField]
        private TMP_Text _priceText;

        public Image Coin => _coin;
        
        private void OnEnable()
        {
            _button.OnClick += OnPlanetButtonClicked;
            _button.OnHold += OnPlanetButtonHeld;
        }

        private void OnDisable()
        {
            _button.OnClick -= OnPlanetButtonClicked;
            _button.OnHold -= OnPlanetButtonHeld;
        }

        public void SetIcon(Sprite sprite) => _icon.sprite = sprite;
        public void DisplayCoin(bool display) => _coin.gameObject.SetActive(display);
        public void DisplayIncome(bool display) => _incomeRoot.SetActive(display);
        public void SetIncomeProgress(float progress) => _incomeProgress.fillAmount = progress;
        public void SetIncomeTimer(string value) => _incomeTimeText.text = value;
        public void SetPrice(string price) => _priceText.text = price;
        
        public void SetPurchaseState(bool state)
        {
            _lock.gameObject.SetActive(!state);
            _priceRoot.SetActive(!state);
        }
        
        private void OnPlanetButtonClicked() => PlanetButtonClicked?.Invoke();
        private void OnPlanetButtonHeld() => PlanetButtonHeld?.Invoke();
    }
}