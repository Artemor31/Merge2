using System.Collections.Generic;
using Databases;
using Infrastructure;
using NaughtyAttributes.Editor.PropertyValidators;
using Services;
using Services.Resources;
using UnityEngine;

namespace UI.UpgradeWindow
{
    public class UpgradeShopPresenter : Presenter
    {
        [SerializeField] private UpgradeItemPresenter _prefab;
        [SerializeField] private RectTransform _parent;
        private UpgradeDataService _upgradeService;
        private UpgradesDatabase _upgradesDatabase;
        private PersistantDataService _persistantService;
        private List<UpgradeItemPresenter> _presenters = new();

        public override void Init()
        {
            _upgradeService = ServiceLocator.Resolve<UpgradeDataService>();
            _persistantService = ServiceLocator.Resolve<PersistantDataService>();
            _upgradesDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UpgradesDatabase>();
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

        public override void OnHide()
        {
            foreach (UpgradeItemPresenter presenter in _presenters)
            {
                Destroy(presenter.gameObject);
            }
            _presenters.Clear();
        }

        private void CreateItem(UpgradeData data)
        {
            UpgradeItemPresenter presenter = Instantiate(_prefab, _parent);
            presenter.SetData(data, _upgradeService);
            _presenters.Add(presenter);
        }
    }
}