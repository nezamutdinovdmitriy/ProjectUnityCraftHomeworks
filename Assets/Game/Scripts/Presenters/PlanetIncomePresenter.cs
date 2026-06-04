using System;
using Game.Presenters;
using Game.Views;
using Modules.Planets;
using Modules.UI;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Presenters
{
    public class PlanetIncomePresenter : MonoBehaviour
    {
        [SerializeField]
        private PlanetIncomeView _view;

        [SerializeField]
        private Transform _transformMoneyViewIcon;

        // Какую зависимость сюда стоило прокинуть, на презентер или на вьюшку, чтобы слушать клик по планете?
        [SerializeField]
        private PlanetPresenter _planetPresenter;
            
        private IPlanet _planet;
        private ParticleAnimator _particleAnimator;

        [Inject]
        public void Construct(ParticleAnimator particleAnimator)
        {
            _particleAnimator = particleAnimator;
        }

        public void Initialize(IPlanet planet)
        {
            _planet = planet;

            _planet.OnIncomeReady += OnIncomeReady;
            _planet.OnIncomeTimeChanged += OnIncomeTimeChanged;
            _planet.OnUnlocked += OnPlanetUnlocked;

            _planetPresenter.PlanetClicked += OnPlanetButtonClicked;

            UpdateState();
        }

        private void UpdateState()
        {
            if (_planet.IsUnlocked == false)
            {
                _view.DisplayIncome(false);
                _view.DisplayCoin(false);
                return;
            }
            
            _view.DisplayCoin(_planet.IsIncomeReady);
            _view.DisplayIncome(!_planet.IsIncomeReady);
        }

        private void OnDisable()
        {
            _planetPresenter.PlanetClicked -= OnPlanetButtonClicked;
            
            _planet.OnIncomeReady -= OnIncomeReady;
            _planet.OnIncomeTimeChanged -= OnIncomeTimeChanged;
            _planet.OnUnlocked -= OnPlanetUnlocked;
        }
        
        private void OnPlanetButtonClicked()
        {
            if (_planet.IsUnlocked && _planet.IsIncomeReady)
            {
                _view.Coin.gameObject.SetActive(false);
                _particleAnimator.Emit(_view.Coin.transform.position, _transformMoneyViewIcon.position);
                _planet.GatherIncome();
            }
        }

        private void OnIncomeTimeChanged(float time)
        {
            TimeSpan incomeTime = TimeSpan.FromSeconds(time);

            _view.SetIncomeProgress(_planet.IncomeProgress);
            _view.SetIncomeTimer($"{(int) incomeTime.TotalMinutes}m:{incomeTime.Seconds:D2}s");
        }

        private void OnPlanetUnlocked() => UpdateState();
        private void OnIncomeReady(bool display) => UpdateState();
    }
}