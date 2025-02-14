using Infrastructure;
using Services.Infrastructure;
using UI.UpgradeWindow;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BottomMenuPresenter : Presenter
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _infoButton;
        
        private WindowsService _windowService;

        public override void Init()
        {
            _windowService = ServiceLocator.Resolve<WindowsService>();
            _shopButton.onClick.AddListener(OpenShop);
            _upgradeButton.onClick.AddListener(OpenUpgrades);
            _playButton.onClick.AddListener(OpenMain);
            _infoButton.onClick.AddListener(OpenInfo);
        }

        private void OpenMain() => CloseAll();

        private void OpenShop()
        {
            CloseAll();
            _windowService.Show<MainShopPresenter>();
        }

        private void OpenInfo()
        {
            CloseAll();
            _windowService.Show<InfoWindowPresenter>();
        }

        private void OpenUpgrades()
        {
            CloseAll();
            _windowService.Show<UpgradeShopPresenter>();
        }

        private void CloseAll()
        {
            _windowService.Close<InfoWindowPresenter>();
            _windowService.Close<UpgradeShopPresenter>();
            _windowService.Close<MainShopPresenter>();
        }
    }
}