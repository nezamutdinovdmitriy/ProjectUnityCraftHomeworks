using System;
using Game.Components;
using SampleGame;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Game.View.Presenters
{
    [DisallowMultipleComponent]
    public sealed class UnitCardsPresenter : MonoBehaviour
    {
        private sealed class CardBinding
        {
            public UnitCardView View;
            public UnitCardConfig Config;
            
            public UnityAction ClickHandler;

            public int LastEnergy = int.MinValue;
            public int LastPrice = int.MinValue;

            public bool CanPurchase;
        }
        
        [SerializeField] private UnitCardsCatalog _catalog;
        [SerializeField] private UnitCardView _cardPrefab;
        [SerializeField] private Transform _container;

        private CardBinding[] _cards = Array.Empty<CardBinding>();

        private World _world;

        private EntityQuery _playerQuery;
        private EntityQuery _timeQuery;
        
        private bool _queriesCreated;
        
        private void Awake()
        {
            if (_catalog == null
                || _cardPrefab == null
                || _container == null
                || _catalog.Cards == null
                || _catalog.Cards.Count == 0)
            {
                Debug.Log("UnitCards: assign a catalog, card prefab and container.");

                enabled = false;
                return;
            }

            foreach (UnitCardConfig config in _catalog.Cards)
            {
                if (config == null)
                {
                    Debug.Log("UnitCards: the catalog contains a missing card.");

                    enabled = false;
                    return;
                }
            }

            _cards = new CardBinding[_catalog.Cards.Count];

            int cardIndex = 0;

            foreach (UnitCardConfig config in _catalog.Cards)
            {
                int itemIndex = cardIndex;

                UnitCardView view = Instantiate(_cardPrefab, _container);

                UnityAction clickHandler = () => OnPurchaseClicked(itemIndex);

                view.SetName(config.Name);
                view.SetIcon(config.Icon);
                view.SetProgressCaption("...");
                view.SetEnabled(false);

                view.OnClicked += clickHandler;

                _cards[cardIndex] = new CardBinding
                {
                    View = view,
                    Config = config,
                    ClickHandler = clickHandler
                };

                cardIndex++;
            }
        }
        
        private bool TryInitializeQueries()
        {
            if (_queriesCreated && _world is {IsCreated: true})
                return true;

            _world = World.DefaultGameObjectInjectionWorld;

            if (_world == null || _world.IsCreated == false)
                return false;

            EntityManager manager = _world.EntityManager;

            _playerQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Player, LocalPlayer, Energy>()
                .WithAllRW<UnitPurchaseRequest>()
                .Build(manager);

            _timeQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<GameTime>()
                .Build(manager);

            _queriesCreated = true;

            return true;
        }
        
        private bool TryReadState(out int energy, out bool canPurchase)
        {
            energy = 0;
            canPurchase = false;

            if (TryInitializeQueries() == false)
                return false;

            _playerQuery.CompleteDependency();
            _timeQuery.CompleteDependency();

            if (_playerQuery.TryGetSingleton(out Energy playerEnergy) == false
                || _timeQuery.TryGetSingleton(out GameTime time) == false)
                return false;

            energy = playerEnergy.Value;
            canPurchase = time.DeltaTime > 0f;

            return true;
        }
        
        private void LateUpdate()
        {
            if (TryReadState(out int energy, out bool canPurchase) == false)
            {
                DisablePurchases();
                return;
            }

            foreach (var card in _cards)
                RefreshCard(card, energy, card.Config.Price, canPurchase);
        }

        private void RefreshCard(
            CardBinding card,
            int energy,
            int price,
            bool canPurchase)
        {
            if (card.LastEnergy != energy || card.LastPrice != price)
            {
                int accumulated = Mathf.Clamp(
                    energy,
                    0,
                    Mathf.Max(0, price));

                float progress = price == 0
                    ? 1f
                    : price > 0
                        ? (float)accumulated / price
                        : 0f;

                string caption = price < 0
                    ? "Недоступно"
                    : price == 0
                        ? "Бесплатно"
                        : $"{accumulated} / {price}";

                card.View.SetProgress(progress);
                card.View.SetProgressCaption(caption);

                card.LastEnergy = energy;
                card.LastPrice = price;
            }

            bool canBuy = canPurchase && price >= 0 && energy >= price;

            if (card.CanPurchase != canBuy)
            {
                card.CanPurchase = canBuy;
                card.View.SetEnabled(canBuy);
            }
        }

        private void OnPurchaseClicked(int itemIndex)
        {
            if (isActiveAndEnabled == false
                || TryReadState(out int energy, out bool canPurchase) == false
                || canPurchase == false)
                return;

            int price = _cards[itemIndex].Config.Price;

            if (price < 0 || energy < price)
                return;
            
            DynamicBuffer<UnitPurchaseRequest> requests = _playerQuery
                .GetSingletonBuffer<UnitPurchaseRequest>();

            requests.Add(new UnitPurchaseRequest
            {
                ItemIndex = itemIndex
            });
        }

        private void DisablePurchases()
        {
            foreach (CardBinding card in _cards)
            {
                if (card == null || card.View == null)
                    continue;

                card.CanPurchase = false;
                card.View.SetEnabled(false);
            }
        }

        private void OnDisable() => DisablePurchases();

        private void OnDestroy()
        {
            foreach (CardBinding card in _cards)
            {
                if (card != null && card.View != null)
                    card.View.OnClicked -= card.ClickHandler;
            }

            if (_queriesCreated && _world is {IsCreated: true})
            {
                _playerQuery.Dispose();
                _timeQuery.Dispose();
            }
        }
    }
}