using System;
using System.Collections.Generic;
using Databases;
using Gameplay.Grid;
using Infrastructure;
using Services;
using Services.GridService;
using Services.Infrastructure;
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
        [SerializeField] public Button Close;
        [SerializeField] public TMP_Text Money;
        [SerializeField] public TMP_Text Wave;
        [SerializeField] public string WaveText;
        [SerializeField] public BuffInfoPresenter BuffPresenter;
        [SerializeField] public SellButton SellButton;

        private Dictionary<UnitCard, ActorConfig> _unitCards;
        private GameplayDataService _gameplayService;
        private GameStateMachine _stateMachine;
        private WindowsService _windowsService;
        private GridLogicService _gridService;
        private UnitsDatabase _unitsDatabase;
        private UnitCard _cardPrefab;
        private bool _refreshed;
        private GridViewService _gridViewService;

        public override void Init()
        {
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _gridService = ServiceLocator.Resolve<GridLogicService>();
            _gameplayService = ServiceLocator.Resolve<GameplayDataService>();
            _stateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _windowsService = ServiceLocator.Resolve<WindowsService>();
            _gridViewService = ServiceLocator.Resolve<GridViewService>();

            StartWaveButton.onClick.AddListener(StartWave);
            GreedButton.onClick.AddListener(AddMoney);
            ShowBuffsButton.onClick.AddListener(BuffsClicked);
            Close.onClick.AddListener(CloseClicked);

            _gameplayService.OnCrownsChanged += OnCrownsChanged;
            _gridViewService.OnPlatformPressed += PlatformPressedHandler;
            _gridViewService.OnPlatformReleased += PlatformReleasedHandler;
            
            CreatePlayerCards();
        }

        private void PlatformReleasedHandler(Platform platform) => SellButton.Hide();

        private void PlatformPressedHandler(Platform platform)
        {
            if (platform.Busy)
            {
                SellButton.Show(_gridService.GetCostFor(platform.Actor.Data.Level));
            }
        }

        public override void OnShow()
        {
            Wave.text = $"{WaveText} {_gameplayService.Wave + 1}";
            OnCrownsChanged(_gameplayService.Crowns);
        }

        private void AddMoney() => _gameplayService.AddCrowns(50);
        private void OnCrownsChanged(int money) => Money.text = money.ToString();
        
        private void CloseClicked()
        {
            _windowsService.Close<GameCanvas>();
            _stateMachine.Enter<ResultScreenState, ResultScreenData>(new ResultScreenData(false, true));
        }
        
        private void BuffsClicked()
        {
            BuffPresenter.gameObject.SetActive(!BuffPresenter.gameObject.activeInHierarchy);
            BuffPresenter.OnShow();
        }

        private void StartWave()
        {
            BuffPresenter.gameObject.SetActive(false);
            gameObject.SetActive(false);
            _stateMachine.Enter<GameLoopState>();
        }

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
            if (_gridService.CanAddUnit())
            {
                _gridService.TryCreatePlayerUnit(_unitCards[card]);
            }
        }
    }
}