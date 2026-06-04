using Game.Scripts.Presenters;
using Game.Views;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class PlanetPresenter : MonoBehaviour
    {
        [SerializeField]
        private PlanetView _view;

        private PlanetPopupPresenter _popupPresenter;
        private IPlanet _planet;

        [Inject]
        public void Construct(PlanetPopupPresenter planetPopupPresenter)
        {
            _popupPresenter = planetPopupPresenter;
        }

        public void Initialize(IPlanet planet)
        {
            _planet = planet;

            _planet.OnUnlocked += OnPlanetUnlocked;

            OnPlanetUnlocked();
        }

        private void OnEnable()
        {
            _view.PlanetButtonClicked += OnPlanetButtonClicked;
            _view.PlanetButtonHeld += OnPlanetButtonHeld;
        }

        private void OnDisable()
        {
            _view.PlanetButtonClicked -= OnPlanetButtonClicked;
            _view.PlanetButtonHeld -= OnPlanetButtonHeld;
            _planet.OnUnlocked -= OnPlanetUnlocked;
        }

        private void OnPlanetButtonHeld()
        {
            if (_planet.IsUnlocked)
                _popupPresenter.Show(_planet);
        }

        private void OnPlanetButtonClicked()
        {
            if (_planet.IsUnlocked == false
                && _planet.CanUnlock)
            {
                _planet.UnlockOrUpgrade();
                UpdateState();
            }
        }

        private void UpdateState()
        {
            _view.SetIcon(_planet.GetIcon(_planet.IsUnlocked));
            _view.SetPrice(_planet.Price.ToString());
        }

        private void OnPlanetUnlocked()
        {
            _view.SetPurchaseState(_planet.IsUnlocked);
            UpdateState();
        }
    }
}