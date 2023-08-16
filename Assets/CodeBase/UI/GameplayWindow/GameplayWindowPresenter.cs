using System.Collections.Generic;
using System.Linq;
using CodeBase.Databases;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
using CodeBase.Infrastructure;
using CodeBase.Models;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.Events;
using Unit = CodeBase.Gameplay.Units.Unit;

namespace CodeBase.UI.GameplayWindow
{
    public class GameplayWindowPresenter
    {
        private readonly GameplayModel _model;
        private readonly GameplayWindow _window;
        private readonly WindowsService _windowsService;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly InputService _inputService;
        private readonly IUpdateable _updateable;

        private readonly Camera _camera;
        private readonly UnitCard _cardPrefab;

        private Dictionary<UnitCard, UnitId> _unitCards;
        private UnitCard _clickedCard;
        private Unit _instanceOfUnit;

        public GameplayWindowPresenter(GameplayModel model,
                                       GameplayWindow window,
                                       WindowsService windowsService,
                                       AssetsProvider assetsProvider,
                                       UnitsDatabase unitsDatabase,
                                       InputService inputService,
                                       IUpdateable updateable)
        {
            _model = model;
            _window = window;
            _windowsService = windowsService;
            _unitsDatabase = unitsDatabase;
            _inputService = inputService;
            _updateable = updateable;

            _camera = Camera.main;
            _cardPrefab = assetsProvider.Load<UnitCard>(AssetsPath.UnitCard);

            _window.StartWave.onClick.AddListener(StartWave);
            _inputService.LeftButtonDown += InputServiceOnLeftButtonUp;
            _updateable.Tick += Update;
            
            CreatePlayerCards();
        }

        private void InputServiceOnLeftButtonUp(Vector3 vector3)
        {
            if (_instanceOfUnit == null) return;
            Object.Instantiate(_instanceOfUnit, _instanceOfUnit.transform.position, Quaternion.identity);
            _instanceOfUnit = null;
            _clickedCard = null;
        }

        private void Update()
        {
            if (_instanceOfUnit != null)
            {
                var position = _inputService.MousePosition;
                var ray = _camera.ScreenPointToRay(position);
                
                bool collided = Physics.Raycast(ray, out var hits, 999f, _window.Ground);
                if (collided == false) return;
                
                position = new Vector3((int)position.x, (int)position.y, (int)position.z);
                _instanceOfUnit.transform.position = position;
            }
        }

        private void CreatePlayerCards()
        {
            _unitCards = new Dictionary<UnitCard, UnitId>();
            foreach (var unitData in _unitsDatabase.Units)
            {
                var card = Object.Instantiate(_cardPrefab, _window.UnitsParent);
                
                card.SetIcon(unitData.Icon);
                card.SetTitle(unitData.Name);
                card.Button.onClick.AddListener(() => CardOnClicked(card)); 
                _unitCards.Add(card, unitData.Id);
            }
        }

        private void CardOnClicked(UnitCard card)
        {
            _clickedCard = card;
            
            var unitPrefab = _unitsDatabase.Units.First(u => u.Id == _unitCards[card]).Prefab;
            _instanceOfUnit = Object.Instantiate(unitPrefab);
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