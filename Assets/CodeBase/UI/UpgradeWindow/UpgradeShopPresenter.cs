using Databases;
using Infrastructure;
using Services;
using Services.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UpgradeWindow
{
    public class UpgradeShopPresenter : Presenter
    {
        [SerializeField] private UpgradeItemPresenter _prefab;
        [SerializeField] private RectTransform _parent;
        [SerializeField] private Button _closeButton;
        private UpgradeDataService _upgradeService;
        private UpgradesDatabase _upgradesDatabase;
        private PersistantDataService _persistantService;

        public override void Init()
        {
            _upgradeService = ServiceLocator.Resolve<UpgradeDataService>();
            _persistantService = ServiceLocator.Resolve<PersistantDataService>();
            _upgradesDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UpgradesDatabase>();
            _closeButton.onClick.AddListener(Close);
        }

        public override void OnShow()
        {
            foreach (UpgradeData data in _upgradesDatabase.Datas)
            {
                if (_persistantService.IsOpened(data.Mastery) || _persistantService.IsOpened(data.Race))
                {
                    CreateItem(data);
                }
            }
        }

        private void CreateItem(UpgradeData data)
        {
            UpgradeItemPresenter presenter = Instantiate(_prefab, _parent);
            presenter.SetData(data, _upgradeService);
        }

        private void Close() => gameObject.SetActive(false);
    }
}