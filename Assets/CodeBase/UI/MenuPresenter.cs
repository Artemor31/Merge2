using Infrastructure;
using Services;
using Services.StateMachine;
using TMPro;
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

        [SerializeField] private TextMeshProUGUI _softText;
        [SerializeField] private TextMeshProUGUI _hardText;

        private GameStateMachine _gameStateMachine;
        private PersistantDataService _persistantDataService;

        public override void Init()
        {
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _persistantDataService = ServiceLocator.Resolve<PersistantDataService>();

            _persistantDataService.OnCoinsChanged += HandleCoinsChanged;
            _persistantDataService.OnGemsChanged += HandleGemsChanged;
            HandleCoinsChanged(_persistantDataService.Coins);
            HandleGemsChanged(_persistantDataService.Gems);

            _fightButton.onClick.AddListener(PlayClicked);
            _addSoftButton.onClick.AddListener(ShowSoftAdsPopup);
            _addHardButton.onClick.AddListener(ShowHardAdsPopup);
        }

        private void ShowHardAdsPopup() => _currencyPopup.ShowData(Currency.Gem);
        private void ShowSoftAdsPopup() => _currencyPopup.ShowData(Currency.Coin);
        private void HandleCoinsChanged(int value) => _softText.text = _persistantDataService.Coins.ToString();
        private void HandleGemsChanged(int value) => _hardText.text = _persistantDataService.Gems.ToString();

        private void PlayClicked()
        {
            gameObject.SetActive(false);
            _gameStateMachine.Enter<LoadLevelState>();
        }
    }
}