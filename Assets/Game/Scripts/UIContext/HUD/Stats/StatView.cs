using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class StatView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text valueText;
        
        [SerializeField]
        private Image progressBar;

        public void SetText(string health)
        {
            this.valueText.text = health;
        }
        
        public void SetProgress(float progress)
        {
            this.progressBar.fillAmount = progress;
        }

        public void SetVisible(bool visible)
        {
            this.gameObject.SetActive(visible);
        }
    }
}