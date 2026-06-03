using System;
using Game.Scripts.Presenters;
using Game.Views;
using Modules.Planets;
using Modules.UI;
using UnityEngine;
using Zenject;

namespace Game.Presenters
{
    public class PlanetPresenter : MonoBehaviour
    {
        [SerializeField]
        private PlanetView _view;

        [SerializeField]
        private Transform _transformMoneyViewIcon;
        
        private PlanetPopupPresenter _popupPresenter;
        
        private IPlanet _planet;
        
        private ParticleAnimator _particleAnimator;

        [Inject]
        public void Construct(
            PlanetPopupPresenter planetPopupPresenter,
            ParticleAnimator particleAnimator)
        {
            _popupPresenter = planetPopupPresenter;
            _particleAnimator = particleAnimator;
        }
        
        public void Initialize(IPlanet planet)
        {
            _planet = planet;
            
            _planet.OnUnlocked += OnPlanetUnlocked;
            _planet.OnIncomeReady += OnIncomeReady;
            _planet.OnIncomeTimeChanged += OnIncomeTimeChanged;
            
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
            _planet.OnIncomeReady -= OnIncomeReady;
            _planet.OnIncomeTimeChanged -= OnIncomeTimeChanged;
        }

        private void OnIncomeTimeChanged(float time)
        {
            TimeSpan incomeTime = TimeSpan.FromSeconds(time);
            
            _view.SetIncomeProgress(_planet.IncomeProgress);
            _view.SetIncomeTimer($"{(int)incomeTime.TotalMinutes}m:{incomeTime.Seconds:D2}s");
        }
        
        private void OnIncomeReady(bool display)
        {
            _view.DisplayIncome(!display);
            _view.DisplayCoin(display);
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
                return;
            }

            if (_planet.IsIncomeReady)
            {
                int income = _planet.MinuteIncome;
                
                _view.Coin.gameObject.SetActive(false);
                _particleAnimator.Emit(_view.Coin.transform.position, _transformMoneyViewIcon.position, 1f);
                _planet.GatherIncome();
            }
        }
        
        private void UpdateState()
        {
            _view.SetIcon(_planet.GetIcon(_planet.IsUnlocked));
            _view.SetPrice(_planet.Price.ToString());

            if (_planet.IsUnlocked)
            {
                OnIncomeReady(_planet.IsIncomeReady);   
            }
        }
        
        private void OnPlanetUnlocked()
        {
            _view.Lock(_planet.IsUnlocked);
            UpdateState();
        }
    }
}