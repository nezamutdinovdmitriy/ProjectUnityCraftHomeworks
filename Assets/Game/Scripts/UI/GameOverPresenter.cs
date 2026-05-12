using Modules.UI;
using UnityEngine;

namespace Game
{
    public class GameOverPresenter : MonoBehaviour
    {
        [SerializeField]
        private Ship _ship;

        [SerializeField]
        private GameOverView _gameOverView;

        private void OnEnable() => _ship.HealthComponent.Dead += _gameOverView.Show;
        private void OnDisable() => _ship.HealthComponent.Dead -= _gameOverView.Show;
        
        
    }
}