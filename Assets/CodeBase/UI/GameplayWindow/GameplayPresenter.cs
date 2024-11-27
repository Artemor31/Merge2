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
        [SerializeField] public SellUnitPresenter SellPresenter;

        private UnitsDatabase _unitsDatabase;
        private UnitCard _cardPrefab;
        private Dictionary<UnitCard, ActorConfig> _unitCards;
        private GridLogicService _gridService;
        private PlayerDataService _playerService;
        private bool _refreshed;
        private GameStateMachine _stateMachine;
        private GridViewService _gridViewService;

        public override void Init()
        {
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _gridService = ServiceLocator.Resolve<GridLogicService>();
            _playerService = ServiceLocator.Resolve<PlayerDataService>();
            _stateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _gridViewService = ServiceLocator.Resolve<GridViewService>();

            StartWaveButton.onClick.AddListener(StartWave);
            GreedButton.onClick.AddListener(AddMoney);
            _playerService.OnMoneyChanged += RuntimeServiceOnOnMoneyChanged;
            _gridViewService.OnPlatformClicked += GridViewServiceOnOnPlatformClicked;
            _gridViewService.OnPlatformReleased += GridViewServiceOnOnPlatformReleased;
            RuntimeServiceOnOnMoneyChanged(_playerService.Money);
            CreatePlayerCards();
        }

        private void GridViewServiceOnOnPlatformClicked(Platform platform)
        {
            if (platform.Actor == null) return;
            ActorConfig actorConfig = _unitsDatabase.ConfigFor(platform.Actor.Data);
            SellPresenter.Show(actorConfig.Cost);
        }

        private void GridViewServiceOnOnPlatformReleased(Platform _) => SellPresenter.Hide();
        private void AddMoney() => _playerService.AddMoney(50);
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
            if (_gridService.CanAddUnit() == false) return;
            if (!_playerService.TryBuy(card.Cost)) return;

            _gridService.TryCreatePlayerUnit(_unitCards[card]);
        }

        private void StartWave()
        {
            gameObject.SetActive(false);
            _stateMachine.Enter<GameLoopState>();
        }
    }
}