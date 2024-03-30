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

        private WindowsService _windowsService;
        private UnitsDatabase _unitsDatabase;
        private UnitCard _cardPrefab;
        private Dictionary<UnitCard, UnitId> _unitCards;
        private bool _refreshed;
        private GameFactory _factory;
        private RuntimeDataProvider _runtimeDataProvider;

        public override void Init()
        {
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>()
                                           .GetDatabase<UnitsDatabase>();

            _runtimeDataProvider = ServiceLocator.Resolve<RuntimeDataProvider>();
            _windowsService = ServiceLocator.Resolve<WindowsService>();
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            _factory = ServiceLocator.Resolve<GameFactory>();

            StartWaveButton.onClick.AddListener(StartWave);
            _runtimeDataProvider.OnMoneyChanged += RuntimeDataProviderOnOnMoneyChanged;
            RuntimeDataProviderOnOnMoneyChanged(_runtimeDataProvider.Money);
            CreatePlayerCards();
        }

        private void RuntimeDataProviderOnOnMoneyChanged(int money) => Money.text = "Money: " + money;

        private void CreatePlayerCards()
        {
            _unitCards = new Dictionary<UnitCard, UnitId>();
            foreach (var unitData in _unitsDatabase.Units)
            {
                UnitCard card = Instantiate(_cardPrefab, UnitsParent);

                card.Setup(unitData);
                card.Button.onClick.AddListener(() => CardOnClicked(card));
                _unitCards.Add(card, unitData.Id);
            }
        }

        private void CardOnClicked(UnitCard card)
        {
            if (_runtimeDataProvider.TryBuy(card.Cost) == false) return;
            
            Platform platform = _runtimeDataProvider.GetFreePlatform();
            if (platform == null) return;
            
            Actor actor = _factory.CreateUnit(_unitCards[card]);
            actor.transform.position = platform.transform.position;
            _runtimeDataProvider.AddActor(actor, platform);
        }

        private void StartWave()
        {
            var playerUnits = _runtimeDataProvider.GetPlayerUnits();
            var enemyUnits = _runtimeDataProvider.EnemyUnits;
            
            foreach (Actor actor in playerUnits) 
                actor.Unleash(enemyUnits);

            foreach (Actor actor in enemyUnits) 
                actor.Unleash(playerUnits);
        }
    }
}