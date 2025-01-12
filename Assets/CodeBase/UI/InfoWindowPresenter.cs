using System.Collections.Generic;
using System.Linq;
using Databases;
using Infrastructure;
using Services;
using Services.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InfoWindowPresenter : Presenter
    {
        [SerializeField] private Transform _actorsParent;
        [SerializeField] private InfoItemPresenter _prefab;
        [SerializeField] private Button _openChest;
        [SerializeField] private TextMeshProUGUI _chestCost;
        [SerializeField] private ChestResultPresenter _chestResult;

        private readonly Dictionary<Race, Dictionary<Mastery, (BuffConfig, bool)>> _datas = new();
        private readonly List<InfoItemPresenter> _items = new();
        private BuffsDatabase _buffsDatabase;
        private UnitsDatabase _unitsDatabase;
        private PersistantDataService _dataService;

        public override void Init()
        {
            _chestCost.text = 100.ToString();
            _buffsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<BuffsDatabase>();
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _dataService = ServiceLocator.Resolve<PersistantDataService>();
            _openChest.onClick.AddListener(OpenChestClicked);
            FillDictionary();
            CreateItems();
        }

        private void OpenChestClicked()
        {
            var closed = from data in _datas 
                         from mastery in data.Value 
                         where !mastery.Value.Item2 
                         select (data.Key, mastery.Key);
            
            _items.ForEach(i => Destroy(i.gameObject));
            _items.Clear();
            
            var random = closed.Random();
            _dataService.SetOpened(random);
            _chestResult.gameObject.SetActive(true);
            _chestResult.OpenChest(random.Item1.ToString() + random.Item2);
            
            CreateItems();
        }

        private void CreateItems()
        {
            CreateItem(Race.Human, _datas[Race.Human]);
            CreateItem(Race.Orc, _datas[Race.Orc]);
            CreateItem(Race.Demon, _datas[Race.Demon]);
            CreateItem(Race.Undead, _datas[Race.Undead]);
        }

        private void FillDictionary()
        {
            foreach (var config in _unitsDatabase.Units)
            {
                BuffConfig buffConfig = _buffsDatabase.MasteryData[config.Data.Mastery];
                bool isOpened = _dataService.IsOpened(config.Data.Mastery, config.Data.Race);
                if (_datas.TryGetValue(config.Data.Race, out var typeDatas))
                {
                    if (!typeDatas.ContainsKey(config.Data.Mastery))
                    {
                        typeDatas.Add(config.Data.Mastery, (buffConfig, isOpened));
                    }
                }
                else
                {
                    var masteries = new Dictionary<Mastery, (BuffConfig, bool)>();
                    masteries.Add(config.Data.Mastery, (buffConfig, isOpened));
                    _datas.Add(config.Data.Race, masteries);
                }
            }
        }

        private void CreateItem(Race race, Dictionary<Mastery, (BuffConfig, bool)> data)
        {
            InfoItemPresenter presenter = Instantiate(_prefab, _actorsParent);
            presenter.SetData(_buffsDatabase.RaceData[race], race, data);
            _items.Add(presenter);
        }
    }
}