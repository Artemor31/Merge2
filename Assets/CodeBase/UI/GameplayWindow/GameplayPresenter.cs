using System.Text;
using Gameplay.Grid;
using Infrastructure;
using Services;
using Services.DataServices;
using Services.Infrastructure;
using Services.StateMachine;
using TMPro;
using UI.WaveSlider;
using UI.WorldSpace;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI.GameplayWindow
{
    public class GameplayPresenter : Presenter
    {
        [SerializeField] public Button StartWaveButton;
        [SerializeField] public Button ShowBuffsButton;
        [SerializeField] public Button Close;
        [SerializeField] public TMP_Text Money;
        [SerializeField] public TMP_Text Wave;
        [SerializeField] public SellButton SellButton;
        [SerializeField] public MenuWaveProgressPresenter MenuWaveProgress;
        [SerializeField] private Button _buyUnit;
        [SerializeField] private TextMeshProUGUI _buffDescription;
        [SerializeField] private GameObject _buffPanel;

        private GridService _gridService;
        private GameplayDataService _gameplayService;
        private GameStateMachine _stateMachine;
        private WindowsService _windowService;
        private BuffService _buffService;

        public override void Init()
        {
            _stateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _gridService = ServiceLocator.Resolve<GridService>();
            _gameplayService = ServiceLocator.Resolve<GameplayDataService>();
            _windowService = ServiceLocator.Resolve<WindowsService>();
            _buffService = ServiceLocator.Resolve<BuffService>();

            _buyUnit.onClick.AddListener(OpenRollWindow);
            StartWaveButton.onClick.AddListener(() => _stateMachine.Enter<GameLoopState>());
            ShowBuffsButton.onClick.AddListener(() => _buffPanel.gameObject.SetActive(!_buffPanel.gameObject.activeInHierarchy));
            Close.onClick.AddListener(() => _stateMachine.Enter<ResultScreenState, ResultScreenData>(ResultScreenData.FastLose));

            _gameplayService.Crowns.AddListener(Money);
            _gridService.OnPlatformReleased += _ => SellButton.Hide();
            _gridService.OnPlatformPressed += PlatformPressedHandler;
            _gridService.OnPlayerFieldChanged += PlayerFieldChanged;
        }

        private void OpenRollWindow()
        {
            _windowService.Show<ActorRollPresenter>();
            _windowService.Close<GameplayPresenter>();
            _windowService.Close<GameCanvas>();
        }

        public override void OnShow()
        {
            Wave.text = $"{WaveText} {_gameplayService.Wave + 1}";
            MenuWaveProgress.Show();
            _buffPanel.gameObject.SetActive(false);
        }

        private void PlatformPressedHandler(Platform platform)
        {
            if (!platform.Busy) return;
            int costFor = _gameplayService.GetCostFor(platform.Data.Level);
            SellButton.Show(costFor);
        }
        
        private void PlayerFieldChanged()
        {
            if (gameObject.activeInHierarchy)
            {
                _buffDescription.text = CreteDescription();
            }
        }

        private string CreteDescription()
        {
            StringBuilder stringBuilder = new();
            var buffs = _buffService.CalculateBuffs(_gridService.PlayerUnits);
            foreach (string buff in buffs)
            {
                stringBuilder.Append(buff);
                stringBuilder.Append("\r\n");
            }
            
            return stringBuilder.ToString();
        }
        
        private string WaveText => YG2.lang switch
        {
            "ru" => "Волна: ",
            "tr" => "Dalga: ",
            _ => "Wave: "
        };
    }
}