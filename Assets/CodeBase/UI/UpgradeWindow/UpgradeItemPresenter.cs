using Databases.BuffConfigs;
using Services.DataServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI.UpgradeWindow
{
    public class UpgradeItemPresenter : Presenter
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _buy;
        [SerializeField] private TextMeshProUGUI _iconName;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _header;
        [SerializeField] private TextMeshProUGUI _description;

        private BuffConfig _config;
        private UpgradeDataService _dataService;

        public void SetData(BuffConfig config, UpgradeDataService dataService)
        {
            _config = config;

            _header.text = config.Name;
            _dataService = dataService;
            SetIcon();
            SetCost();
            SetDescription();
        }

        private void SetIcon()
        {
            if (_config.Icon != null)
            {
                _icon.sprite = _config.Icon;
                _icon.enabled = true;
                _iconName.enabled = false;
            }
            else
            {
                _iconName.text = _config.Name;
                _icon.enabled = false;
                _iconName.enabled = true;
            }
        }

        private void Buy()
        {
            if (_dataService.TryProgress(_config.Name))
            {
                SetCost();
                SetDescription();
            }
        }

        private void SetDescription() => _description.text = YG2.lang switch
        {
            "ru" => $"текущий бонус: +{_dataService.LevelOf(_config.Name)}%",
            "tr" => $"Güncel bonus: +{_dataService.LevelOf(_config.Name)}%",
            _ => $"Current bonus: +{_dataService.LevelOf(_config.Name)}%"
        };

        private void OnEnable() => _buy.onClick.AddListener(Buy);
        private void SetCost() => _cost.text = _dataService.CalculateCostForLevel(_dataService.LevelOf(_config.Name) + 1).ToString();
        private void OnDisable() => _buy.onClick.RemoveListener(Buy);
    }
}