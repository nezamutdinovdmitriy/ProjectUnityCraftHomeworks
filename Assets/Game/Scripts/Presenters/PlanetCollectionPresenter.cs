using System;
using System.Collections.Generic;
using Game.Presenters;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class PlanetCollectionPresenter : MonoBehaviour
    {
        private List<IPlanet> _planets;

        [SerializeField]
        private List<PlanetPresenter> _planetPresenters;
        
        [Inject]
        public void Construct(
            List<IPlanet> planets,
            List<PlanetPresenter> planetPresenters)
        {
            _planets = planets;
            _planetPresenters = planetPresenters;
        }

        private void Start()
        {
            Dictionary<string, IPlanet> planetsMapping = new();

            foreach (IPlanet planet in _planets)
                planetsMapping.Add(planet.Name, planet);

            foreach (PlanetPresenter presenter in _planetPresenters)
            {
                if (planetsMapping.TryGetValue(presenter.Name, out IPlanet planet) == false)
                    throw new InvalidOperationException($"Planet {presenter.Name} not found!");
                
                presenter.Initialize(planet);
            }
        }
    }
}