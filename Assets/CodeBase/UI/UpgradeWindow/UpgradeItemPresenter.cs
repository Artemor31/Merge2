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
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _header;
        [SerializeField] private TextMeshProUGUI _description;

        private UpgradeData _config;
        private UpgradeDataService _dataService;

        public void SetData(UpgradeData config, UpgradeDataService dataService)
        {
            _config = config;
            _icon.sprite = config.Icon;
            _header.text = config.Name;
            _dataService = dataService;
            SetCost();
            SetDescription();
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
        private void SetCost() => _cost.text = _dataService.GetUpgradeLevel(_config.Name).ToString();
        private void OnDisable() => _buy.onClick.RemoveListener(Buy);
    }
}