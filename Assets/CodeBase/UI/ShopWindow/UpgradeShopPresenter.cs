using System;
using System.Linq;
using Databases;
using Infrastructure;
using Services;
using Services.SaveService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShopWindow
{

    [Serializable]
    public class UpgradeProgress
    {
        public UpgradeProgressPair[] UpgradesProgress;
    }
    
    [Serializable]
    public class UpgradeProgressPair
    {
        public string Id;
        public int Level;
    }
    
    public class UpgradeDataService : IService
    {
        private const string SavePath = "UpgradesData";
        
        private readonly PlayerDataService _playerDataService;
        private readonly SaveService _saveService;
        private readonly UpgradeProgress _progress;

        public UpgradeDataService(PlayerDataService playerDataService, SaveService saveService)
        {
            _playerDataService = playerDataService;
            _saveService = saveService;
            _progress = _saveService.Restore<UpgradeProgress>(SavePath);
        }

        public bool TryProgress(string id)
        {
            UpgradeProgressPair upgrade = _progress.UpgradesProgress.First(p => p.Id == id);
            var cost = GetUpgradeCost(upgrade.Level + 1);
            
            if (_playerDataService.TryBuy(cost))
            {
                upgrade.Level++;
                _saveService.Save(SavePath, _progress);
                return true;
            }
            return false;
        }

        public int GetUpgradeCost(string id) => GetUpgradeCost(_progress.UpgradesProgress.First(p => p.Id == id).Level);
        private int GetUpgradeCost(float x) => (int)(Math.Log10(x + 1) * 2 + x / 6);
    }
    
    public class UpgradeItemPresenter : Presenter
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _header;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Button _buy;
        [SerializeField] private TextMeshProUGUI _cost;
        
        private PlayerDataService _playerDataService;
        private ActorConfig _config;
        private UpgradeDataService _service;

        private void OnEnable()
        {
            _buy.onClick.AddListener(Buy);
            _service = ServiceLocator.Resolve<UpgradeDataService>();
        }

        public void SetData(ActorConfig config)
        {
            _config = config;
            _icon.sprite = config.ViewData.Icon;
            _header.text = config.Name;
            SetCost();
        }

        private void Buy()
        {
            if (_service.TryProgress(_config.Name))
            {
                SetCost();
            }
        }

        private void SetCost() => _cost.text = _service.GetUpgradeCost(_config.Name).ToString();
        private void OnDisable() => _buy.onClick.RemoveListener(Buy);
    }
    
    public class UpgradeShopPresenter : Presenter
    {
        [SerializeField] private UpgradeItemPresenter _itemPresenter;
        [SerializeField] private RectTransform _parent;
    }
}