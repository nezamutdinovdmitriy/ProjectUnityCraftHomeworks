using System;
using Game.Components;
using SampleGame;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game.View.Presenters
{
    [DisallowMultipleComponent]
    public sealed class UnitCardsPresenter : MonoBehaviour
    {
        [SerializeField] private UnitCardsCatalog _catalog;
        [SerializeField] private UnitCardView _cardPrefab;
        [SerializeField] private Transform _container;

        private UnitCardPresenter[] _unitCardPresenters = Array.Empty<UnitCardPresenter>();

        private World _world;
        private EntityQuery _playerQuery;
        private EntityQuery _gameStateQuery;
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

            _unitCardPresenters = new UnitCardPresenter[_catalog.Cards.Count];

            int itemIndex = 0;

            foreach (UnitCardConfig config in _catalog.Cards)
            {
                UnitCardView view = Instantiate(_cardPrefab, _container);

                _unitCardPresenters[itemIndex] = new UnitCardPresenter(view, config);
                itemIndex++;
            }
        }

        private void OnDisable() => DisablePurchases();
        
        private void OnDestroy()
        {
            if (!_queriesCreated || _world is not {IsCreated: true})
                return;

            EntityManager manager = _world.EntityManager;

            if (manager.IsQueryValid(_playerQuery))
                _playerQuery.Dispose();

            if (manager.IsQueryValid(_gameStateQuery))
                _gameStateQuery.Dispose();
        }
        
        private void LateUpdate()
        {
            if (!TryReadState(out int energy, out bool purchasesAllowed))
            {
                DisablePurchases();
                return;
            }

            for (int i = 0; i < _unitCardPresenters.Length; i++)
            {
                UnitCardPresenter unitCardPresenter = _unitCardPresenters[i];
                unitCardPresenter.Refresh(energy, purchasesAllowed);

                if (unitCardPresenter.TryConsumePurchaseClick())
                    EnqueuePurchase(i);
            }
        }
        
        private bool TryInitializeQueries()
        {
            if (_queriesCreated && _world is {IsCreated: true})
                return true;

            _world = World.DefaultGameObjectInjectionWorld;

            if (_world is not {IsCreated: true})
                return false;

            EntityManager manager = _world.EntityManager;

            _playerQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Player, LocalPlayer, Energy>()
                .WithAllRW<UnitPurchaseRequest>()
                .Build(manager);

            _gameStateQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<GameState>()
                .Build(manager);
            
            _queriesCreated = true;

            return true;
        }

        private bool TryReadState(out int energy, out bool purchasesAllowed)
        {
            energy = 0;
            purchasesAllowed = false;
            
            if (!TryInitializeQueries())
                return false;

            _playerQuery.CompleteDependency();
            _gameStateQuery.CompleteDependency();

            if (!_playerQuery.TryGetSingleton(out Energy playerEnergy)
                || !_gameStateQuery.TryGetSingleton(out GameState state))
                return false;
            
            energy = playerEnergy.Value;
            purchasesAllowed = state.Value == GameStateType.Playing;

            return true;
        }

        private void EnqueuePurchase(int itemIndex)
        {
            DynamicBuffer<UnitPurchaseRequest> requests = _playerQuery
                .GetSingletonBuffer<UnitPurchaseRequest>();

            requests.Add(new UnitPurchaseRequest
            {
                ItemIndex = itemIndex
            });
        }

        private void DisablePurchases()
        {
            foreach (UnitCardPresenter cardBinding in _unitCardPresenters)
                cardBinding.DisablePurchase();
        }
    }
}