using System.Collections.Generic;
using CodeBase.Databases;
using CodeBase.Gameplay.Units;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using CodeBase.Services;
using CodeBase.Services.SaveService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.GameplayWindow
{
    public class GameplayPresenter : Presenter
    {
        [SerializeField] public Transform UnitsParent;
        [SerializeField] public Button StartWaveButton;
        [SerializeField] public TMP_Text Money;

        private UnitsDatabase _unitsDatabase;
        private UnitCard _cardPrefab;
        private Dictionary<UnitCard, (Race, Mastery)> _unitCards;
        private bool _refreshed;
        private GameFactory _factory;
        private GridDataService _gridDataService;
        private GameObserver _observer;
        private PlayerProgressService _playerService;

        public override void Init()
        {
            _factory = ServiceLocator.Resolve<GameFactory>();
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _gridDataService = ServiceLocator.Resolve<GridDataService>();
            _playerService = ServiceLocator.Resolve<PlayerProgressService>();
            _observer = ServiceLocator.Resolve<GameObserver>();

            StartWaveButton.onClick.AddListener(StartWave);
            _playerService.OnMoneyChanged += RuntimeServiceOnOnMoneyChanged;
            RuntimeServiceOnOnMoneyChanged(_playerService.Money);
            CreatePlayerCards();
        }

        private void RuntimeServiceOnOnMoneyChanged(int money) => Money.text = "Money: " + money;

        private void CreatePlayerCards()
        {
            _unitCards = new Dictionary<UnitCard, (Race, Mastery)>();
            foreach (var unitData in _unitsDatabase.Units)
            {
                UnitCard card = Instantiate(_cardPrefab, UnitsParent);

                card.Setup(unitData);
                card.Button.onClick.AddListener(() => CardClicked(card));
                _unitCards.Add(card, (unitData.Race, unitData.Mastery));
            }
        }

        private void CardClicked(UnitCard card)
        {
            Platform platform = _gridDataService.GetFreePlatform();
            if (platform == null) return;

            if (!_playerService.TryBuy(card.Cost)) return;
            
            Actor actor = _factory.CreateActor(_unitCards[card].Item1, _unitCards[card].Item2);
            actor.transform.position = platform.transform.position;
            _gridDataService.AddPlayerUnit(actor, platform);
        }

        private void StartWave()
        {
            _observer.StartWatch();
            _gridDataService.Save();
            var playerUnits = _gridDataService.GetPlayerUnits();
            var enemyUnits = _gridDataService.EnemyUnits;
            
            foreach (Actor actor in playerUnits) 
                actor.Unleash(enemyUnits);

            foreach (Actor actor in enemyUnits) 
                actor.Unleash(playerUnits);
            
            gameObject.SetActive(false);
        }
    }
}