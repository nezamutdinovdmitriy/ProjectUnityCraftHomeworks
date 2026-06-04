using Game.Views;
using Modules.Planets;
using UnityEngine;

namespace Game.Scripts.Presenters
{
    public class PlanetPopupPresenter : MonoBehaviour
    {
        [SerializeField]
        private PlanetPopupView _view;

        private IPlanet _selectedPlanet;

        private void OnEnable()
        {
            _view.CloseButtonClicked += OnCloseButtonClicked;
            _view.UpgradeButtonClicked += OnUpgradeButtonClicked;
        }

        private void OnDisable()
        {
            _view.CloseButtonClicked -= OnCloseButtonClicked;
            _view.UpgradeButtonClicked -= OnUpgradeButtonClicked;
        }

        public void Show(IPlanet planet)
        {
            _selectedPlanet = planet;

            _selectedPlanet.OnPopulationChanged += OnPopulationChanged;

            UpdateState();
            _view.Show();
        }

        public void Hide()
        {
            _selectedPlanet.OnPopulationChanged -= OnPopulationChanged;
            
            _view.Hide();
            _selectedPlanet = null;
        }

        private void UpdateState()
        {
            _view.SetTitle(_selectedPlanet.Name);
            _view.SetAvatar(_selectedPlanet.GetIcon(_selectedPlanet.IsUnlocked));
            
            _view.SetPopulation($"Population: {_selectedPlanet.Population}");
            _view.SetLevel($"Level: {_selectedPlanet.Level} / {_selectedPlanet.MaxLevel}");
            _view.SetIncome($"Income: {_selectedPlanet.MinuteIncome} / sec");
            _view.SetPrice(_selectedPlanet.Price.ToString());
            
            UpdateUpgradeButton();
        }

        private void UpdateUpgradeButton()
        {
            if (_selectedPlanet.IsMaxLevel)
            {
                _view.SetUpgradeButtonText("MAX LEVEL");
                _view.SetInteractableUpgradeButton(false);
                _view.PriceVisible(false);
            }
            else
            {
                _view.SetUpgradeButtonText("UPGRADE");
                _view.SetInteractableUpgradeButton(_selectedPlanet.CanUpgrade);
                _view.PriceVisible(true);
            }
        }

        private void OnPopulationChanged(int value) => _view.SetPopulation($"Population: {value}");

        private void OnUpgradeButtonClicked()
        {
            if (_selectedPlanet.CanUpgrade
                && _selectedPlanet.IsMaxLevel == false)
                _selectedPlanet.Upgrade();

            UpdateState();
        }

        private void OnCloseButtonClicked() => Hide();
    }
}