using System.Collections.Generic;
using System.Linq;
using CodeBase.Databases;
using CodeBase.Databases.Data;
using CodeBase.Gameplay;
using CodeBase.LevelData;
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
        private readonly LevelStaticData _levelStaticData;

        private readonly UnitCard _cardPrefab;

        private Dictionary<UnitCard, UnitId> _unitCards;
        private bool _refreshed;

        public GameplayWindowPresenter(GameplayModel model,
                                       GameplayWindow window,
                                       WindowsService windowsService,
                                       AssetsProvider assetsProvider,
                                       UnitsDatabase unitsDatabase,
                                       LevelStaticData levelStaticData)
        {
            _model = model;
            _window = window;
            _windowsService = windowsService;
            _unitsDatabase = unitsDatabase;
            _levelStaticData = levelStaticData;

            _cardPrefab = assetsProvider.Load<UnitCard>(AssetsPath.UnitCard);

            _window.StartWave.onClick.AddListener(StartWave);
            CreatePlayerCards();
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
            var unitPrefab = _unitsDatabase.Units.First(u => u.Id == _unitCards[card]).Prefab;
            var position = _levelStaticData.PlayerPositions.First();
            var unit = Object.Instantiate(unitPrefab, position, Quaternion.AngleAxis(180, Vector3.up));
            _model.PlayerUnits.Add(unit);
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