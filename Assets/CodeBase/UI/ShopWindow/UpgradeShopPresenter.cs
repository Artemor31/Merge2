using System.Collections.Generic;
using Infrastructure;
using Services.SaveService;
using TMPro;
using UnityEngine;

namespace UI.ShopWindow
{
    public class UpgradeShopPresenter : Presenter
    {
        [SerializeField] private UpgradeItemPresenter _itemPresenter;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private TextMeshProUGUI _coins;

        private PersistantDataService _dataService;
        private UpgradeDataService _upgradeService;
        private List<UpgradeItemPresenter> _items;

        public override void Init()
        {
            _upgradeService = ServiceLocator.Resolve<UpgradeDataService>();
            _dataService = ServiceLocator.Resolve<PersistantDataService>();
            _items = new List<UpgradeItemPresenter>();
        }

        private void OnEnable()
        {
            CreateItems();
            SetCurrency();
        }

        private void OnDisable()
        {
            Clear();
        }

        private void Clear()
        {
            foreach (var item in _items)
            {
                Destroy(item.gameObject);
            }
            
            _items.Clear();
        }

        private void SetCurrency() => _coins.text = _dataService.Coins.ToString();

        private void CreateItems()
        {
            foreach (var config in _upgradeService.CurrentConfigs())
            {
                var instance = Instantiate(_itemPresenter, _parent);
                instance.SetData(config, _upgradeService);
                _items.Add(instance);
            }
        }
    }
}