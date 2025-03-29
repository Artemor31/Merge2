using System.Collections.Generic;
using Databases;
using Gameplay.Grid;
using Infrastructure;
using Services;
using Services.DataServices;
using Services.Infrastructure;
using Services.StateMachine;
using TMPro;
using UI.WaveSlider;
using UnityEngine;
using UnityEngine.UI;
using YG;

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
        [SerializeField] public BuffInfoPresenter BuffPresenter;
        [SerializeField] public SellButton SellButton;
        [SerializeField] public MenuWaveProgressPresenter MenuWaveProgress;

        private GridService _gridService;
        private Dictionary<UnitCard, ActorConfig> _unitCards;
        private GameplayDataService _gameplayService;
        private GameStateMachine _stateMachine;
        private UnitsDatabase _unitsDatabase;
        private UnitCard _cardPrefab;
        private bool _refreshed;

        public override void Init()
        {
            _gameplayService = ServiceLocator.Resolve<GameplayDataService>();
            _stateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _gridService = ServiceLocator.Resolve<GridService>();

            StartWaveButton.onClick.AddListener(() => _stateMachine.Enter<GameLoopState>());
            ShowBuffsButton.onClick.AddListener(() => BuffPresenter.Show());
            Close.onClick.AddListener(CloseClicked);

            _gameplayService.OnCrownsChanged += OnCrownsChanged;
            _gridService.OnPlatformPressed += PlatformPressedHandler;
            _gridService.OnPlatformReleased += PlatformReleasedHandler;

#if UNITY_EDITOR
            GreedButton.onClick.AddListener(AddMoney);
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _cardPrefab = ServiceLocator.Resolve<AssetsProvider>().Load<UnitCard>(AssetsPath.UnitCard);
            CreatePlayerCards();
#endif
        }

        public override void OnShow()
        {
            Wave.text = $"{WaveText} {_gameplayService.Wave + 1}";
            OnCrownsChanged(_gameplayService.Crowns);
            MenuWaveProgress.Show();
        }

        private void PlatformPressedHandler(Platform platform)
        {
            if (!platform.Busy) return;
            int costFor = _gameplayService.GetCostFor(platform.Data.Level);
            SellButton.Show(costFor);
        }

        private void PlatformReleasedHandler(Platform platform) => SellButton.Hide();
        private void AddMoney() => _gameplayService.AddCrowns(50);
        private void OnCrownsChanged(int money) => Money.text = money.ToString();
        private void CloseClicked() => _stateMachine.Enter<ResultScreenState, ResultScreenData>(new ResultScreenData(false, true));

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
            if (_gridService.CanAddUnit)
            {
                _gridService.TryCreatePlayerUnit(_unitCards[card]);
            }
        }
        
        private string WaveText => YG2.lang switch
        {
            "ru" => "Волна: ",
            "tr" => "Dalga: ",
            _ => "Wave: "
        };
    }
}