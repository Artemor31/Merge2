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
        private Dictionary<UnitCard, UnitId> _unitCards;
        private bool _refreshed;
        private GameFactory _factory;
        private RuntimeDataRepository _runtimeDataRepository;
        private GameObserver _observer;

        public override void Init()
        {
            _factory = ServiceLocator.Resolve<GameFactory>();
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _runtimeDataRepository = ServiceLocator.Resolve<RuntimeDataRepository>();
            _observer = ServiceLocator.Resolve<GameObserver>();

            StartWaveButton.onClick.AddListener(StartWave);
            _runtimeDataRepository.OnMoneyChanged += RuntimeDataRepositoryOnOnMoneyChanged;
            RuntimeDataRepositoryOnOnMoneyChanged(_runtimeDataRepository.Money);
            CreatePlayerCards();
        }

        private void RuntimeDataRepositoryOnOnMoneyChanged(int money) => Money.text = "Money: " + money;

        private void CreatePlayerCards()
        {
            _unitCards = new Dictionary<UnitCard, UnitId>();
            foreach (var unitData in _unitsDatabase.Units)
            {
                UnitCard card = Instantiate(_cardPrefab, UnitsParent);

                card.Setup(unitData);
                card.Button.onClick.AddListener(() => CardClicked(card));
                _unitCards.Add(card, unitData.Id);
            }
        }

        private void CardClicked(UnitCard card)
        {
            Platform platform = _runtimeDataRepository.GetFreePlatform();
            if (platform == null) return;

            if (!_runtimeDataRepository.TryBuy(card.Cost)) return;
            
            Actor actor = _factory.CreateActor(_unitCards[card]);
            actor.transform.position = platform.transform.position;
            _runtimeDataRepository.AddPlayerUnit(actor, platform);
        }

        private void StartWave()
        {
            _observer.StartWatch();
            _runtimeDataRepository.Save();
            var playerUnits = _runtimeDataRepository.GetPlayerUnits();
            var enemyUnits = _runtimeDataRepository.EnemyUnits;
            
            foreach (Actor actor in playerUnits) 
                actor.Unleash(enemyUnits);

            foreach (Actor actor in enemyUnits) 
                actor.Unleash(playerUnits);
            
            gameObject.SetActive(false);
        }
    }
}