using System.Collections.Generic;
using Databases;
using Infrastructure;
using Services.Resources;
using UnityEngine;

namespace UI.UpgradeWindow
{
    public class InfoWindowPresenter : Presenter
    {
        [SerializeField] private Transform _actorsParent;
        [SerializeField] private InfoItemPresenter _prefab;

        private List<InfoItemPresenter> _items = new();
        private BuffsDatabase _buffsDatabase;
        private UnitsDatabase _unitsDatabase;

        public override void Init()
        {
            _buffsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<BuffsDatabase>();
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            
            Dictionary<Race, Dictionary<Mastery, BuffConfig>> dictionary = new();
            
            foreach (var config in _unitsDatabase.Units)
            {
                if (dictionary.TryGetValue(config.Data.Race, out var typeDatas))
                {
                    if (!typeDatas.ContainsKey(config.Data.Mastery))
                    {
                        typeDatas.Add(config.Data.Mastery, _buffsDatabase.MasteryData[config.Data.Mastery]);
                    }
                }
                else
                {
                    var datas = new Dictionary<Mastery, BuffConfig>();
                    datas.Add(config.Data.Mastery, _buffsDatabase.MasteryData[config.Data.Mastery]);
                    dictionary.Add(config.Data.Race, datas);
                }
            }

            foreach (var data in dictionary)
            {
                var presenter = Instantiate(_prefab, _actorsParent);
                var info = _buffsDatabase.RaceData[data.Key];
                presenter.SetData(info.Icon, info.Name, info.Description, data.Value);
                _items.Add(presenter);
            }
        }
    }
}