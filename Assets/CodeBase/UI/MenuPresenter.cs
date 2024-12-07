using Infrastructure;
using Services.StateMachine;
using TMPro;
using UI.ShopWindow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MenuWindow
{
    public class MenuPresenter : Presenter
    {
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _upgradeButton;
        
        private GameStateMachine _gameStateMachine;
        private WindowsService _windowService;

        public override void Init()
        {
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _windowService = ServiceLocator.Resolve<WindowsService>();

            _playButton.onClick.AddListener(PlayClicked);
            _upgradeButton.onClick.AddListener(OpenUpgrades);
        }

        private void OpenUpgrades() => _windowService.Show<UpgradeShopPresenter>();

        private void PlayClicked()
        {
            gameObject.SetActive(false);
            _playButton.onClick.RemoveListener(PlayClicked);
            _gameStateMachine.Enter<LoadLevelState>();
        }
    }
}