using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Views
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _value;

        [SerializeField]
        private RectTransform _scaleTarget;

        [SerializeField]
        private float _scalePunchIntensity = 0.15f;
        
        [SerializeField]
        private float _scaleDuration = 0.2f;
        
        public void SetValue(string value) => _value.text = value;

        public void PlayAnimation()
        {
            if (_scaleTarget == null)
                return;
            
            _scaleTarget.DOKill();
            _scaleTarget.localScale = Vector3.one;
            _scaleTarget.DOPunchScale(
                new Vector3(
                    _scalePunchIntensity,
                    _scalePunchIntensity,
                    0f),
                _scaleDuration, 1, 0.5f);
        }

        public void ResetAnimation()
        {
            if (_scaleTarget == null)
                return;

            _scaleTarget.DOKill();
            _scaleTarget.localScale = Vector3.one;
        }
    }
}