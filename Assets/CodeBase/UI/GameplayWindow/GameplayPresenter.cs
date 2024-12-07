using System.Collections.Generic;
using Databases;
using Gameplay.LevelItems;
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
        [SerializeField] public Button GreedButton;
        [SerializeField] public TMP_Text Money;
        [SerializeField] public ActorMenuPresenter ActorMenu;

        private Dictionary<UnitCard, ActorConfig> _unitCards;
        private GridViewService _gridViewService;
        private GameplayDataService _gameplayService;
        private GameStateMachine _stateMachine;
        private GridLogicService _gridService;
        private UnitsDatabase _unitsDatabase;
        private UnitCard _cardPrefab;
        private bool _refreshed;
        private CameraService _cameraService;

        public override void Init()
        {
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _gridService = ServiceLocator.Resolve<GridLogicService>();
            _gameplayService = ServiceLocator.Resolve<GameplayDataService>();
            _stateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _gridViewService = ServiceLocator.Resolve<GridViewService>();
            _cameraService = ServiceLocator.Resolve<CameraService>();

            StartWaveButton.onClick.AddListener(StartWave);
            GreedButton.onClick.AddListener(AddMoney);

            _gridViewService.OnPlatformClicked += OnPlatformClicked;
            _gridViewService.OnPlatformPressed += OnPlatformPressed;
            _gameplayService.OnMoneyChanged += OnMoneyChanged;
            OnMoneyChanged(_gameplayService.Gold);
            CreatePlayerCards();
        }

        private void OnPlatformClicked(Platform platform)
        {
            if (platform.Busy)
            {
                var screenPoint = _cameraService.WorldToScreenPoint(platform.transform.position);
                var actorConfig = _unitsDatabase.ConfigFor(platform.Data);
                ActorMenu.Show(platform, screenPoint, actorConfig);
            }
        }

        private void OnPlatformPressed(Platform platform) => ActorMenu.Hide();
        private void AddMoney() => _gameplayService.AddMoney(50);
        private void OnMoneyChanged(int money) => Money.text = "Money: " + money;

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
            if (_gridService.CanAddUnit() && _gameplayService.TryBuy(card.Cost))
            {
                _gridService.TryCreatePlayerUnit(_unitCards[card]);
            }
        }

        private void StartWave()
        {
            gameObject.SetActive(false);
            _stateMachine.Enter<GameLoopState>();
        }
    }
}