using Infrastructure;
using Services;
using Services.DataServices;
using Services.Infrastructure;
using Services.StateMachine;
using UI.WaveSlider;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI.MenuWindow
{
    public class MenuPresenter : Presenter
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _storyFightButton;
        [SerializeField] private Button _addHardButton;
        [SerializeField] private Button _addSoftButton;
        [SerializeField] private CurrencyAddPopup _currencyPopup;
        [SerializeField] private MenuWaveProgressPresenter _slider;
        [SerializeField] private Button _diamondsUp;
        [SerializeField] private Button _patchShow;

        private GameStateMachine _gameStateMachine;

        public override void Init()
        {
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
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

        public override void OnShow() => _slider.Show();
        
        private void ShowPatch() => ServiceLocator.Resolve<WindowsService>().Show<SimpleInfoWindow>();
        private void ShowHardAdsPopup() => _currencyPopup.ShowData(Currency.Gem);
        private void ShowSoftAdsPopup() => _currencyPopup.ShowData(Currency.Coin);
        
        private void PlayStoryClicked()
        {
            gameObject.SetActive(false);
            _gameStateMachine.Enter<LoadLevelState>();
        }
    }
}