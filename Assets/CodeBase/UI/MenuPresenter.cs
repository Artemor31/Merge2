using Infrastructure;
using Services;
using Services.StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuPresenter : Presenter
    {
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _addHardButton;
        [SerializeField] private Button _addSoftButton;
        [SerializeField] private CurrencyAddPopup _currencyPopup;
        [SerializeField] private Button _gridUp;
        [SerializeField] private Button _coinsUp;
        
        private GameStateMachine _gameStateMachine;

        public override void Init()
        {
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _fightButton.onClick.AddListener(PlayClicked);
            _addSoftButton.onClick.AddListener(ShowSoftAdsPopup);
            _addHardButton.onClick.AddListener(ShowHardAdsPopup);
            _gridUp.onClick.AddListener(() => ServiceLocator.Resolve<PersistantDataService>().UpRows());
            _coinsUp.onClick.AddListener(() => ServiceLocator.Resolve<PersistantDataService>().UpCrowns());
        }

        private void ShowHardAdsPopup() => _currencyPopup.ShowData(Currency.Gem);
        private void ShowSoftAdsPopup() => _currencyPopup.ShowData(Currency.Coin);

        private void PlayClicked()
        {
            gameObject.SetActive(false);
            _gameStateMachine.Enter<LoadLevelState>();
        }
    }
}