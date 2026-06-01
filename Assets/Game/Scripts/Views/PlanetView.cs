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
        
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Image _lock;

        [SerializeField]
        private Image _coin;

        [SerializeField]
        private Image _progressBar;

        [SerializeField]
        private TMP_Text _timerText;

        [SerializeField]
        private TMP_Text _priceText;

        [SerializeField]
        private SmartButton _button;

        private void OnEnable() => _button.OnClick += OnPlanetButtonClicked;
        private void OnDisable() => _button.OnClick -= OnPlanetButtonClicked;

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void SetIcon(Sprite sprite) => _icon.sprite = sprite;
        
        public void SetLock(Sprite sprite) => _lock.sprite = sprite;
        
        public void SetPrice(string price) => _priceText.text = price;
        
        private void OnPlanetButtonClicked() => PlanetButtonClicked?.Invoke();
    }
}