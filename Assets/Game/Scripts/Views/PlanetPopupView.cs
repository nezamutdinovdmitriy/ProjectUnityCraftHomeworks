using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlanetPopupView : MonoBehaviour
    {
        public event Action CloseButtonClicked;
        public event Action UpgradeButtonClicked;
        
        [Header("Header")] [SerializeField]
        private TMP_Text _title;

        [SerializeField]
        private Button _closeButton;

        [Header("Body")] [SerializeField]
        private Image _avatar;

        [Header("Population")] [SerializeField]
        private TMP_Text _populationText;

        [Header("Level")] [SerializeField]
        private TMP_Text _levelText;

        [Header("Income")] [SerializeField]
        private TMP_Text _incomeText;

        [Header("Upgrade")] [SerializeField]
        private Button _upgradeButton;

        [SerializeField]
        private TMP_Text _priceText;

        private void OnEnable()
        {
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveListener(OnUpgradeButtonClicked);
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void SetTitle(string title) => _title.text = title;

        public void SetAvatar(Sprite sprite) => _avatar.sprite = sprite;

        public void SetPopulation(string population) => _populationText.text = population;

        public void SetLevel(string level) => _levelText.text = level;

        public void SetIncome(string income) => _incomeText.text = income;

        public void SetPrice(string price) => _priceText.text = price;
        
        private void OnCloseButtonClicked() => CloseButtonClicked?.Invoke();
        private void OnUpgradeButtonClicked() => UpgradeButtonClicked?.Invoke();

    }
}