using System;
using System.Collections.Generic;
using Game.Presenters;
using Game.Scripts.Presenters;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class PlanetPresenterInitializer : MonoBehaviour
    {
        private List<IPlanet> _planets;
        private List<PlanetPresenter> _planetPresenters;
        private List<PlanetIncomePresenter> _incomePresenters;

        [Inject]
        public void Construct(
            List<IPlanet> planets,
            List<PlanetPresenter> planetPresenters,
            List<PlanetIncomePresenter> incomePresenters)
        {
            _planets = planets;
            _planetPresenters = planetPresenters;
            _incomePresenters = incomePresenters;
        }

        private void Start()
        {
            if (_planetPresenters.Count != _planets.Count
                || _incomePresenters.Count != _planets.Count)
                throw new InvalidOperationException();
            
            
            for (int i = 0; i < _planetPresenters.Count; i++)
            {
                _planetPresenters[i].Initialize(_planets[i]);
                _incomePresenters[i].Initialize(_planets[i]);
            }
        }
    }
}