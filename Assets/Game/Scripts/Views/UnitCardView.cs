using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SampleGame
{
    public sealed class UnitCardView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _progress;
        [SerializeField] private ProgressBarPro _progressBar;
        [SerializeField] private Button _button;

        private bool _clickRequired;

        private void OnEnable() => _button.onClick.AddListener(OnClicked);
        private void OnDisable() => _button.onClick.RemoveListener(OnClicked);

        public bool ConsumeClick()
        {
            if (!_clickRequired)
                return false;

            _clickRequired = false;
            return true;
        }

        public void SetIcon(Sprite icon) => _icon.sprite = icon;

        public void SetName(string unitName) => _name.text = unitName;

        public void SetProgressCaption(string progress) => _progress.text = progress;

        public void SetProgress(float value) => _progressBar.SetValue(value);

        public void SetEnabled(bool state) => _button.interactable = state;

        private void OnClicked() => _clickRequired = true;
    }
}