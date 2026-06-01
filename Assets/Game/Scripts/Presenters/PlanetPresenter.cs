using System;
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

        [SerializeField]
        private PlanetConfig _config;
        
        private IPlanet _planet;

        public void Initialize(IPlanet planet)
        {
            _planet = planet;
            
            UpdateState();
        }
        
        private void OnEnable()
        {
            _view.PlanetButtonClicked += OnPlanetButtonClicked;
        }

        private void OnDisable()
        {
            _view.PlanetButtonClicked -= OnPlanetButtonClicked;
        }
        
        private void OnPlanetButtonClicked()
        {
            Debug.Log("Planet View Clicked");
        }
        
        private void UpdateState()
        {
            Debug.Log("UpdateState Invoked");
            
            /*_view.SetIcon(_planet.GetIcon(_planet.IsUnlocked));
            _view.SetPrice(_planet.Price.ToString());*/
        }
    }
}