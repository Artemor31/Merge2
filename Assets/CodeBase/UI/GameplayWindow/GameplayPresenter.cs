using System.Collections.Generic;
using Databases;
using Gameplay.Units;
using Infrastructure;
using Services;
using Services.SaveService;
using Services.StateMachine;
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
        private GridDataService _gridDataService;
        private PlayerProgressService _playerService;
        private WaveBuilder _waveBuilder;
        private bool _refreshed;
        private GameStateMachine _stateMachine;

        public override void Init()
        {
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _gridDataService = ServiceLocator.Resolve<GridDataService>();
            _waveBuilder = ServiceLocator.Resolve<WaveBuilder>();
            _playerService = ServiceLocator.Resolve<PlayerProgressService>();
            _stateMachine = ServiceLocator.Resolve<GameStateMachine>();

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
            if (_gridDataService.HasFreePlatform() == false) return;
            if (!_playerService.TryBuy(card.Cost)) return;

            _gridDataService.AddPlayerUnit(_unitCards[card]);
        }

        private void StartWave()
        {
            _stateMachine.Enter<GameLoopState>();
            gameObject.SetActive(false);
        }
    }
}