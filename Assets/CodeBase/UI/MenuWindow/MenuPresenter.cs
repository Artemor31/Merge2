using Infrastructure;
using Services.Infrastructure;
using Services.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MenuWindow
{
    public class MenuPresenter : Presenter
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _storyFightButton;
        [SerializeField] private Button _addHardButton;
        [SerializeField] private Button _addSoftButton;
        [SerializeField] private CurrencyAddPopup _currencyPopup;
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
        }

        private void ShowPatch() => ServiceLocator.Resolve<WindowsService>().Show<SimpleInfoWindow>();
        private void ShowHardAdsPopup() => _currencyPopup.ShowData(Currency.Gem);
        private void ShowSoftAdsPopup() => _currencyPopup.ShowData(Currency.Coin);
        private void PlayStoryClicked() => _gameStateMachine.Enter<LoadLevelState>();
    }
}