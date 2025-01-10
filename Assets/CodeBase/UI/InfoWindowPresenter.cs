using System.Collections.Generic;
using Databases;
using Infrastructure;
using Services.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InfoWindowPresenter : Presenter
    {
        [SerializeField] private Transform _actorsParent;
        [SerializeField] private InfoItemPresenter _prefab;
        [SerializeField] private Button _openChest;

        private List<InfoItemPresenter> _items = new();
        private BuffsDatabase _buffsDatabase;
        private UnitsDatabase _unitsDatabase;

        public override void Init()
        {
            _buffsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<BuffsDatabase>();
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _openChest.onClick.AddListener(OpenChestClicked);
            
            Dictionary<Race, Dictionary<Mastery, BuffConfig>> dictionary = new();
            FillDictionary(dictionary);
            CreateItem(Race.Human, dictionary[Race.Human]);
            CreateItem(Race.Orc, dictionary[Race.Orc]);
            CreateItem(Race.Demon, dictionary[Race.Demon]);
            CreateItem(Race.Undead, dictionary[Race.Undead]);
        }

        private void OpenChestClicked()
        {
            
        }

        private void FillDictionary(Dictionary<Race, Dictionary<Mastery, BuffConfig>> dictionary)
        {
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
        }

        private void CreateItem(Race race, Dictionary<Mastery, BuffConfig> data)
        {
            InfoItemPresenter presenter = Instantiate(_prefab, _actorsParent);
            presenter.SetData(_buffsDatabase.RaceData[race], race, data);
            _items.Add(presenter);
        }
    }
}