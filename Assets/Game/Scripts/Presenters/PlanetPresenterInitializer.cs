using System.Collections.Generic;
using Game.Presenters;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class PlanetPresenterInitializer : MonoBehaviour
    {
        [SerializeField]
        private PlanetPresenter[] _presenters;

        private List<IPlanet> _planets;

        [Inject]
        public void Construct(List<IPlanet> planets) 
            => _planets = planets;

        private void Start()
        {
            for (int i = 0; i < _presenters.Length; i++)
                _presenters[i].Initialize(_planets[i]);
        }
    }
}