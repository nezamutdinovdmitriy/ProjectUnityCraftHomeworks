using System;
using Modules.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlanetView : MonoBehaviour
    {
        public event Action PlanetButtonClicked
        {
            add => _button.OnClick += value;
            remove => _button.OnClick -= value;
        }
        public event Action PlanetButtonHeld
        {
            add => _button.OnHold += value;
            remove => _button.OnHold -= value;
        }

        [SerializeField]
        private PlanetIncomeView _incomeView;
        
        [SerializeField]
        private SmartButton _button;
        
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Image _lock;

        [Header("Price")] [SerializeField]
        private GameObject _priceRoot;

        [SerializeField]
        private Image _priceIcon;

        [SerializeField]
        private TMP_Text _priceText;
        
        public void SetPurchaseState(bool state)
        {
            _lock.gameObject.SetActive(!state);
            _priceRoot.SetActive(!state);
        }
        
        public void SetIcon(Sprite sprite) => _icon.sprite = sprite;
        public void SetPrice(string price) => _priceText.text = price;
    }
}