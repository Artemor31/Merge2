using System.Collections.Generic;
using Databases;
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
        [SerializeField] public Button Close;
        [SerializeField] public TMP_Text Money;
        [SerializeField] public TMP_Text Wave;
        [SerializeField] public string WaveText;
        [SerializeField] public BuffInfoPresenter BuffPresenter;

        private Dictionary<UnitCard, ActorConfig> _unitCards;
        private GameplayDataService _gameplayService;
        private GameStateMachine _stateMachine;
        private GridLogicService _gridService;
        private UnitsDatabase _unitsDatabase;
        private UnitCard _cardPrefab;
        private bool _refreshed;

        public override void Init()
        {
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _gridService = ServiceLocator.Resolve<GridLogicService>();
            _gameplayService = ServiceLocator.Resolve<GameplayDataService>();
            _stateMachine = ServiceLocator.Resolve<GameStateMachine>();

            StartWaveButton.onClick.AddListener(StartWave);
            GreedButton.onClick.AddListener(AddMoney);
            ShowBuffsButton.onClick.AddListener(BuffsClicked);
            Close.onClick.AddListener(CloseClicked);

            _gameplayService.OnCrownsChanged += OnCrownsChanged;
            OnCrownsChanged(_gameplayService.Crowns);
            CreatePlayerCards();
        }

        private void CloseClicked()
        {
            
            _stateMachine.Enter<ResultScreenState, bool>(false);
        }

        public override void OnShow()
        {
            Wave.text = $"{WaveText} {_gameplayService.Wave}";
        }

        private void BuffsClicked()
        {
            BuffPresenter.gameObject.SetActive(!BuffPresenter.gameObject.activeInHierarchy);
            BuffPresenter.OnShow();
        }

        private void AddMoney() => _gameplayService.AddCrowns(50);
        private void OnCrownsChanged(int money) => Money.text = money.ToString();

        private void StartWave()
        {
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