using System.Collections.Generic;
using Databases;
using Databases.BuffConfigs;
using Infrastructure;
using Services;
using Services.Resources;
using UnityEngine;

namespace UI.UpgradeWindow
{
    public class UpgradeShopPresenter : Presenter
    {
        [SerializeField] private UpgradeItemPresenter _prefab;
        [SerializeField] private RectTransform _parent;
        
        private readonly List<UpgradeItemPresenter> _presenters = new();
        private UpgradeDataService _upgradeService;
        private BuffsDatabase _actorTypesDatabase;
        private PersistantDataService _persistantService;

        public override void Init()
        {
            _upgradeService = ServiceLocator.Resolve<UpgradeDataService>();
            _persistantService = ServiceLocator.Resolve<PersistantDataService>();
            _actorTypesDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<BuffsDatabase>();
        }

        public override void OnShow()
        {
            foreach (BuffConfig data in _actorTypesDatabase.BuffConfigs)
            {
                if (Opened(data))
                {
                    CreateItem(data);
                }
            }
        }

        private bool Opened(BuffConfig data) => data.Mastery == Mastery.None 
            ? _persistantService.IsOpened(data.Race) 
            : _persistantService.IsOpened(data.Mastery);

        public override void OnHide()
        {
            foreach (UpgradeItemPresenter presenter in _presenters)
            {
                Destroy(presenter.gameObject);
            }
            
            _presenters.Clear();
        }

        private void CreateItem(BuffConfig data)
        {
            UpgradeItemPresenter presenter = Instantiate(_prefab, _parent);
            presenter.SetData(data, _upgradeService);
            _presenters.Add(presenter);
        }
    }
}