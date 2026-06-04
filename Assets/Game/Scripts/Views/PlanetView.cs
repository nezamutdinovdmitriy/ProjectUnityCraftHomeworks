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

        [Header("Price")] [SerializeField]
        private GameObject _priceRoot;

        [SerializeField]
        private Image _priceIcon;

        [SerializeField]
        private TMP_Text _priceText;

        
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
        
        public void SetPurchaseState(bool state)
        {
            _lock.gameObject.SetActive(!state);
            _priceRoot.SetActive(!state);
        }
        
        public void SetIcon(Sprite sprite) => _icon.sprite = sprite;
        public void SetPrice(string price) => _priceText.text = price;
        
        private void OnPlanetButtonClicked() => PlanetButtonClicked?.Invoke();
        private void OnPlanetButtonHeld() => PlanetButtonHeld?.Invoke();
    }
}