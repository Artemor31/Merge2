using Infrastructure;
using Services.Infrastructure;
using UI.UpgradeWindow;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BottomMenuPresenter : Presenter
    {
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _infoButton;
        
        private WindowsService _windowService;

        public override void Init()
        {
            _windowService = ServiceLocator.Resolve<WindowsService>();
            _upgradeButton.onClick.AddListener(OpenUpgrades);
            _playButton.onClick.AddListener(OpenMain);
            _infoButton.onClick.AddListener(OpenInfo);
        }

        private void OpenInfo()
        {
            _windowService.Close<UpgradeShopPresenter>();
            _windowService.Show<InfoWindowPresenter>();
        }

        private void OpenMain()
        {
            _windowService.Close<UpgradeShopPresenter>();
            _windowService.Close<InfoWindowPresenter>();
        }

        private void OpenUpgrades()
        {
            _windowService.Close<InfoWindowPresenter>();
            _windowService.Show<UpgradeShopPresenter>();
        }
    }
}