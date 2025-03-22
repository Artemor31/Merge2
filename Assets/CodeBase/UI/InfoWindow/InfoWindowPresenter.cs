using System.Collections.Generic;
using System.Linq;
using Databases;
using Databases.BuffConfigs;
using Infrastructure;
using Services.DataServices;
using Services.Infrastructure;
using Services.ProgressData;
using UnityEngine;

namespace UI.InfoWindow
{
    public class InfoWindowPresenter : Presenter
    {
        [SerializeField] private Transform _actorsParent;
        [SerializeField] private InfoItemPresenter _prefab;
        [SerializeField] private Race[] _raceQueue = {Race.Human, Race.Orc, Race.Undead, Race.Demon};

        private readonly List<InfoItemPresenter> _items = new();
        private BuffsDatabase _buffsDatabase;
        private UnitsDatabase _unitsDatabase;
        private PersistantDataService _dataService;

        public override void Init()
        {
            _buffsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<BuffsDatabase>();
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _dataService = ServiceLocator.Resolve<PersistantDataService>();
            _dataService.OnProgressChanged += HandleProgressChanged;
            CreateItems();
        }

        private void HandleProgressChanged(PersistantProgress progress) => CreateItems();

        private void CreateItems()
        {
            _items.ForEach(i => Destroy(i.gameObject));
            _items.Clear();

            foreach (Race race in _raceQueue)
            {
                CreateItem(race);
            }
        }

        private void CreateItem(Race race)
        {
            var masteries = _unitsDatabase.AllActorTypes()[race];
            IEnumerable<(BuffConfig, bool)> data = masteries.Select(mastery => CheckOpened(race, mastery));

            InfoItemPresenter presenter = Instantiate(_prefab, _actorsParent);
            presenter.SetData(_buffsDatabase.RaceData[race], data, _dataService.IsOpened(race));
            _items.Add(presenter);
        }

        private (BuffConfig, bool) CheckOpened(Race race, Mastery mastery) =>
            (_buffsDatabase.MasteryData[mastery], _dataService.IsOpened(mastery, race));
    }
}