using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlanetIncomeView : MonoBehaviour
    {
        [SerializeField]
        private Image _coin;

        [Header("Income")] [SerializeField]
        private GameObject _incomeRoot;

        [SerializeField]
        private Image _incomeProgress;

        [SerializeField]
        private TMP_Text _incomeTimeText;
        
        public Image Coin => _coin;
        
        public void DisplayCoin(bool display) => _coin.gameObject.SetActive(display);
        public void DisplayIncome(bool display) => _incomeRoot.SetActive(display);
        public void SetIncomeProgress(float progress) => _incomeProgress.fillAmount = progress;
        public void SetIncomeTimer(string value) => _incomeTimeText.text = value;
    }
}