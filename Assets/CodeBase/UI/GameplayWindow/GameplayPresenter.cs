using System.Collections.Generic;
using Databases;
using Gameplay.Grid;
using Infrastructure;
using Services;
using Services.GridService;
using Services.Resources;
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
        [SerializeField] public Button ShowBuffsButton;
        [SerializeField] public TMP_Text Money;
        [SerializeField] public ActorMenuPresenter ActorMenu;
        [SerializeField] public BuffInfoPresenter BuffPresenter;

        private Dictionary<UnitCard, ActorConfig> _unitCards;
        private GridViewService _gridViewService;
        private GameplayDataService _gameplayService;
        private GameStateMachine _stateMachine;
        private GridLogicService _gridService;
        private UnitsDatabase _unitsDatabase;
        private CameraService _cameraService;
        private UnitCard _cardPrefab;
        private bool _refreshed;

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
            ShowBuffsButton.onClick.AddListener(BuffsClicked);

            _gridViewService.OnPlatformClicked += OnPlatformClicked;
            _gridViewService.OnPlatformPressed += OnPlatformPressed;
            _gameplayService.OnCrownsChanged += OnCrownsChanged;
            OnCrownsChanged(_gameplayService.Crowns);
            CreatePlayerCards();
        }

        private void BuffsClicked()
        {
            BuffPresenter.gameObject.SetActive(!BuffPresenter.gameObject.activeInHierarchy);
            BuffPresenter.OnShow();
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
        private void AddMoney() => _gameplayService.AddCrowns(50);
        private void OnCrownsChanged(int money) => Money.text = money.ToString();

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