using System.Collections.Generic;
using System.Linq;
using CodeBase.Databases;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using CodeBase.Infrastructure;
using CodeBase.Models;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.UI.GameplayWindow
{
    public class GameplayWindowPresenter
    {
        private readonly GameplayModel _model;
        private readonly GameplayWindow _window;
        private readonly WindowsService _windowsService;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly InputService _inputService;

        private Dictionary<UnitCard, UnitId> _cards;
        private readonly Camera _camera;

        private UnitCard _clickedCard;
        private Unit _instanceOfUnit;
        private UnitCard _cardPrefab;

        public GameplayWindowPresenter(GameplayModel model,
                                       GameplayWindow window,
                                       WindowsService windowsService,
                                       AssetsProvider assetsProvider,
                                       UnitsDatabase unitsDatabase,
                                       InputService inputService)
        {
            _model = model;
            _window = window;
            _windowsService = windowsService;
            _unitsDatabase = unitsDatabase;
            _inputService = inputService;

            _camera = Camera.main;
            _cardPrefab = assetsProvider.Load<UnitCard>(AssetsPath.UnitCard);

            _window.StartWave.onClick.AddListener(StartWave);
            _inputService.LeftButtonDown += InputServiceOnLeftButtonDown;
            _inputService.LeftButtonUp += InputServiceOnLeftButtonUp;

            CreatePlayerCards();
        }

        private void CreatePlayerCards()
        {
            _cards = new Dictionary<UnitCard, UnitId>();
            foreach (var unitData in _unitsDatabase.Units)
            {
                var card = Object.Instantiate(_cardPrefab, _window.UnitsParent);
                
                card.SetIcon(unitData.Icon);
                card.SetTitle(unitData.Name);
                card.Clicked += CardOnClicked;
                _cards.Add(card, unitData.Id);
            }
        }

        private void CardOnClicked(UnitCard card)
        {
            _clickedCard = card;
            var unitId = _cards[card];
            var unitPrefab = _unitsDatabase.Units.First(u => u.Id == unitId).Prefab;
            var unit = Object.Instantiate(unitPrefab);
            _model.PlayerUnits.Add(unit);
        }

        private void InputServiceOnLeftButtonDown(Vector3 vector3)
        {
            var ray = _camera.ScreenPointToRay(vector3);
            bool collided = Physics.Raycast(ray, out var hits, 999f, _window.LayerMask);
            if (collided)
            {
                _instanceOfUnit.transform.position = hits.point;
            }
        }

        private void InputServiceOnLeftButtonUp(Vector3 vector3)
        {
            Object.Instantiate(_instanceOfUnit, _instanceOfUnit.transform.position, Quaternion.identity);
        }

        private void StartWave()
        {
            _model.State = GameState.Processing;
            _window.SetBattleState();
        }

        private void OpenShowWindow()
        {
            _windowsService.Show<ShopWindow>();
        }

        private void UpdateHp()
        {
        }

        private void UpdateMoney()
        {
        }

        private void UpdateCards()
        {
        }
    }
}