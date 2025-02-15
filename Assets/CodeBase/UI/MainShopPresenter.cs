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
    public class MainShopPresenter : Presenter
    {
        private const int ChestCost = 100;
        private const string Max = "MAX";
        private const string StartCrowns = "Начальные короны: +";
        private const string StartGrid = "Количество рядов: ";

        [SerializeField] private Button _openChest;
        [SerializeField] private Button _buyGrid;
        [SerializeField] private Button _buyCoins;
        [SerializeField] private TextMeshProUGUI _chestCost;
        [SerializeField] private TextMeshProUGUI _gridCost;
        [SerializeField] private TextMeshProUGUI _coinsCost;
        [SerializeField] private TextMeshProUGUI _crownsDescription;
        [SerializeField] private TextMeshProUGUI _gridDescription;
        [SerializeField] private ChestResultPresenter _chestResult;
        
        private BuffsDatabase _buffsDatabase;
        private UnitsDatabase _unitsDatabase;
        private PersistantDataService _dataService;

        public override void Init()
        {
            var provider = ServiceLocator.Resolve<DatabaseProvider>();
            _buffsDatabase = provider.GetDatabase<BuffsDatabase>();
            _unitsDatabase = provider.GetDatabase<UnitsDatabase>();
            _dataService = ServiceLocator.Resolve<PersistantDataService>();
            
            _openChest.onClick.AddListener(OpenChestClicked);
            _buyGrid.onClick.AddListener(OpenGridClicked);
            _buyCoins.onClick.AddListener(OpenCoinsClicked);
            _chestCost.text = ChestCost.ToString();
            _gridCost.text = ChestCost.ToString();
            _coinsCost.text = ChestCost.ToString();
            UpdateCrownDescr();
            UpdateRowsDescr();
        }

        private void UpdateRowsDescr() => _gridDescription.text = StartGrid + (_dataService.Rows+1);
        private void UpdateCrownDescr() => _crownsDescription.text = StartCrowns + _dataService.Crowns;

        private void OpenCoinsClicked()
        {
            if (!_dataService.TryBuyGems(ChestCost)) return;
            if (!_dataService.TryUpCrowns())
            {
                _coinsCost.text = Max;
            }
            else
            {
                UpdateCrownDescr();
            }
        }

        private void OpenGridClicked()
        {
            if (!_dataService.TryBuyGems(ChestCost)) return;
            if (!_dataService.TryUpRows())
            {
                _gridCost.text = Max;
            }
            else
            {
                UpdateRowsDescr();
            }
        }

        private void OpenChestClicked()
        {
            if (!_dataService.TryBuyGems(ChestCost)) return;

            string text;
            List<(Race, Mastery)> closed = GetClosedTypes();

            if (closed.Count == 0)
            {
                text = "Все типы уже открыты.\r\nЖдите обновлений!";
            }
            else
            {
                bool inTutor = !_dataService.IsOpened(Race.Human, Mastery.Ranger);
                (Race, Mastery) selection = inTutor ? (Race.Human, Mastery.Ranger) : closed.Random();

                _dataService.SetOpened(selection);
                text = $"{_buffsDatabase.NameFor(selection.Item1)} {_buffsDatabase.NameFor(selection.Item2)}";
                _chestResult.SetRace(_buffsDatabase.IconFor(selection.Item1));
                _chestResult.SetMastery(_buffsDatabase.IconFor(selection.Item2));
            }

            _chestResult.gameObject.SetActive(true);
            _chestResult.SetText(text);
        }

        private List<(Race, Mastery)> GetClosedTypes()
        {
            Dictionary<Race,IEnumerable<Mastery>> allActorTypes = _unitsDatabase.AllActorTypes();
            var selectMany = allActorTypes.SelectMany(dict => dict.Value, (keyValuePair, mastery) => new {keyValuePair, mastery});
            var enumerable = selectMany.Where(t => !_dataService.IsOpened(t.mastery, t.keyValuePair.Key));
            IEnumerable<(Race Key, Mastery mastery)> valueTuples = enumerable.Select(t => (t.keyValuePair.Key, t.mastery));
            return valueTuples.ToList();
        }
    }
}