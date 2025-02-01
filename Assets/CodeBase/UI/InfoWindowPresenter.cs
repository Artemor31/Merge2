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
        private const int ChestCost = 100;
        [SerializeField] private Transform _actorsParent;
        [SerializeField] private InfoItemPresenter _prefab;
        [SerializeField] private Button _openChest;
        [SerializeField] private TextMeshProUGUI _chestCost;
        [SerializeField] private ChestResultPresenter _chestResult;
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
            _openChest.onClick.AddListener(OpenChestClicked);
            _chestCost.text = ChestCost.ToString();
            CreateItems();
        }

        private void OpenChestClicked()
        {
            if (!_dataService.TryBuyGems(ChestCost)) return;

            string text;
            List<(Race, Mastery)> closed = AllClosed();

            if (closed.Count == 0)
            {
                text = "Все типы уже открыты. Ждите обновлений!";
            }
            else
            {
                ClearItems();

                bool inTutor = !_dataService.IsOpened(Race.Human, Mastery.Ranger);
                (Race, Mastery) selection = inTutor ? (Race.Human, Mastery.Ranger) : closed.Random();

                _dataService.SetOpened(selection);
                text = selection.Item1.ToString() + selection.Item2;
                CreateItems();
            }

            _chestResult.gameObject.SetActive(true);
            _chestResult.SetData(text);
        }

        private List<(Race, Mastery)> AllClosed() =>
            _unitsDatabase.AllActorTypes()
                          .SelectMany(keyValuePair =>
                              keyValuePair.Value, (keyValuePair, mastery) =>
                              new {keyValuePair, mastery})
                          .Where(t => !_dataService.IsOpened(t.mastery, t.keyValuePair.Key))
                          .Select(t => (t.keyValuePair.Key, t.mastery))
                          .ToList();

        private void ClearItems()
        {
            _items.ForEach(i => Destroy(i.gameObject));
            _items.Clear();
        }

        private void CreateItems()
        {
            foreach (Race race in _raceQueue)
            {
                CreateItem(race);
            }
        }

        private void CreateItem(Race race)
        {
            IEnumerable<(BuffConfig, bool)> data = _unitsDatabase.AllActorTypes()[race]
                                                                 .Select(mastery => CheckOpened(race, mastery));

            InfoItemPresenter presenter = Instantiate(_prefab, _actorsParent);
            presenter.SetData(_buffsDatabase.RaceData[race], data);
            _items.Add(presenter);
        }

        private (BuffConfig, bool) CheckOpened(Race race, Mastery mastery) =>
            (_buffsDatabase.MasteryData[mastery], _dataService.IsOpened(mastery, race));
    }
}