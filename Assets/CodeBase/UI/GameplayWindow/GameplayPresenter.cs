using System.Collections.Generic;
using Databases;
using Gameplay.Units;
using Infrastructure;
using LevelData;
using Services;
using Services.SaveService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class GameplayPresenter : Presenter
    {
        [SerializeField] public Transform UnitsParent;
        [SerializeField] public Button StartWaveButton;
        [SerializeField] public TMP_Text Money;

        private UnitsDatabase _unitsDatabase;
        private UnitCard _cardPrefab;
        private Dictionary<UnitCard, ActorConfig> _unitCards;
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
            _unitCards = new Dictionary<UnitCard, ActorConfig>();
            foreach (ActorConfig actorConfig in _unitsDatabase.Units)
            {
                UnitCard card = Instantiate(_cardPrefab, UnitsParent);

                card.Setup(actorConfig);
                card.Button.onClick.AddListener(() => CardClicked(card));
                _unitCards.Add(card, actorConfig);
            }
        }

        private void CardClicked(UnitCard card)
        {
            Platform platform = _gridDataService.GetFreePlatform();
            if (platform == null) return;

            if (!_playerService.TryBuy(card.Cost)) return;

            Actor actor = _factory.CreateActor(_unitCards[card]);
            actor.transform.position = platform.transform.position;
            _gridDataService.AddPlayerUnit(actor, platform);
        }

        private void StartWave()
        {
            _gridDataService.Save();
            _observer.StartWatch();
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