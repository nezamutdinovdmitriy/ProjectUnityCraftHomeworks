using SampleGame;
using UnityEngine;

namespace Game.View.Presenters
{
    public sealed class UnitCardPresenter
    {
        private readonly UnitCardView _view;
        private readonly UnitCardConfig _config;

        private int _lastEnergy = int.MinValue;
        private int _lastPrice = int.MinValue;
        private bool _canPurchase;

        public UnitCardPresenter(UnitCardView view, UnitCardConfig config)
        {
            _view = view;
            _config = config;

            _view.SetName(_config.Name);
            _view.SetIcon(_config.Icon);
            _view.SetProgressCaption("...");
            _view.SetEnabled(false);
        }

        public void Refresh(int energy, bool purchasesAllowed)
        {
            int price = _config.Price;

            if (_lastEnergy != energy || _lastPrice != price)
            {
                RefreshProgress(energy, price);
                _lastEnergy = energy;
                _lastPrice = price;
            }

            bool canPurchase = purchasesAllowed &&
                               price >= 0 &&
                               energy >= price;

            SetPurchaseEnabled(canPurchase);
        }

        public bool TryConsumePurchaseClick()
        {
            bool clicked = _view.ConsumeClick();
            return clicked && _canPurchase;
        }

        public void DisablePurchase()
        {
            if (_view == null)
                return;

            SetPurchaseEnabled(false);
            _view.ConsumeClick();
        }

        private void SetPurchaseEnabled(bool canPurchase)
        {
            if (_canPurchase == canPurchase)
                return;

            _canPurchase = canPurchase;
            _view.SetEnabled(canPurchase);
        }

        private void RefreshProgress(int energy, int price)
        {
            if (price < 0)
            {
                _view.SetProgress(0f);
                _view.SetProgressCaption("Unavailable");
                return;
            }

            if (price == 0)
            {
                _view.SetProgress(1f);
                _view.SetProgressCaption("Free");
                return;
            }

            int accumulated = Mathf.Clamp(energy, 0, price);

            _view.SetProgress((float) accumulated / price);
            _view.SetProgressCaption($"{accumulated} / {price}");
        }
    }
}