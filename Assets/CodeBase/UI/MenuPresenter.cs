using Services.StateMachine;
using Services.GridService;
using UnityEngine.UI;
using Infrastructure;
using UnityEngine;
using Services;
using Services.Infrastructure;
using UI.ResultWindow;
using YG;

namespace UI
{
    public class MenuPresenter : Presenter
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _storyFightButton;
        [SerializeField] private Button _addHardButton;
        [SerializeField] private Button _addSoftButton;
        [SerializeField] private CurrencyAddPopup _currencyPopup;
        [SerializeField] private WaveProgressPopup _waveRewardPopup;
        [SerializeField] private MenuWaveProgressPresenter _slider;
        [SerializeField] private Button _diamondsUp;
        [SerializeField] private Button _patchShow;

        private GameStateMachine _gameStateMachine;
        private GameplayDataService _gameplayDataService;
        private GridDataService _gridDataService;

        public override void Init()
        {
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _gameplayDataService = ServiceLocator.Resolve<GameplayDataService>();
            _gridDataService = ServiceLocator.Resolve<GridDataService>();
            
            //_fightButton.onClick.AddListener(PlayClicked);
            _storyFightButton.onClick.AddListener(PlayStoryClicked);
            _addSoftButton.onClick.AddListener(ShowSoftAdsPopup);
            _addHardButton.onClick.AddListener(ShowHardAdsPopup);
            _patchShow.onClick.AddListener(ShowPatch);
            
            //_diamondsUp.onClick.AddListener(() => ServiceLocator.Resolve<PersistantDataService>().AddGems(1000));

            YG2.StickyAdActivity(true);
            if (!ServiceLocator.Resolve<TutorialService>().SeenTutor &&
                ServiceLocator.Resolve<PersistantDataService>().Coins == 100)
            {
                YG2.StickyAdActivity(false);
            }
        }

        private void ShowPatch() => ServiceLocator.Resolve<WindowsService>().Show<SimpleInfoWindow>();

        public override void OnShow() => _slider.Show();
        private void ShowHardAdsPopup() => _currencyPopup.ShowData(Currency.Gem);
        private void ShowSoftAdsPopup() => _currencyPopup.ShowData(Currency.Coin);

        // ne zashlo bratan4ik
        // private void PlayClicked()
        // {
        //     gameObject.SetActive(false);
        //     
        //     _gameplayDataService.SelectMode(false);
        //     _gameplayDataService.Reset();
        //     _gridDataService.Reset();
        //     
        //     _gameStateMachine.Enter<LoadLevelState>();
        // }
        
        private void PlayStoryClicked()
        {
            gameObject.SetActive(false);
            
            _gameplayDataService.SelectMode(true);
            
            _gameStateMachine.Enter<LoadLevelState>();
        }
    }
}