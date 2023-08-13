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

        private readonly Dictionary<UnitCard, Unit> _cards;
        private readonly Camera _camera;

        private UnitCard _clickedCard;
        private Unit _instanceOfUnit;
        private UnitCard _prefab;

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
            _prefab = assetsProvider.Load<UnitCard>(AssetsPath.UnitCard);

            _window.StartWave.onClick.AddListener(StartWave);
            _inputService.LeftButtonDown += InputServiceOnLeftButtonDown;
            _inputService.LeftButtonUp += InputServiceOnLeftButtonUp;

            _cards = new Dictionary<UnitCard, Unit>();
            CreatePlayerCards();
        }

        private void CreatePlayerCards()
        {
            var units = _unitsDatabase.Units.Select(u => u.Prefab);
            foreach (var unit in units)
            {
                var createdUnit = Object.Instantiate(unit, _window.UnitsParent);
                _model.PlayerUnits.Add(createdUnit);
            }
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

        private void UnitCardOnClicked(UnitCard card)
        {
            _clickedCard = card;
            var original = _cards[_clickedCard];
            _instanceOfUnit = Object.Instantiate(original);
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