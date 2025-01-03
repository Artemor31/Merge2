using Databases;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        private ActorTypeData _config;
        private UpgradeDataService _dataService;

        public void SetData(ActorTypeData config, UpgradeDataService dataService)
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

        private void OnEnable() => _buy.onClick.AddListener(Buy);
        private void SetDescription() => _description.text = "current level is " + _dataService.LevelOf(_config.Name);
        private void SetCost() => _cost.text = _dataService.CalculateCostForLevel(_dataService.LevelOf(_config.Name)).ToString();
        private void OnDisable() => _buy.onClick.RemoveListener(Buy);
    }
}