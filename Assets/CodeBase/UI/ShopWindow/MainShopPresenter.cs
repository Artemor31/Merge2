using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Infrastructure;
using Services.DataServices;
using Services.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI.ShopWindow
{
    public class MainShopPresenter : Presenter
    {
        private const int ChestCost = 100;
        private const string Max = "MAX";

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
            SetChestCost();
            SetCrownsCost();
            SetRowsCost();
            UpdateCrownDescr();
            UpdateRowsDescr();
        }

        public override void OnShow() => YG2.InterstitialAdvShow();
        private void UpdateRowsDescr() => _gridDescription.text = StartGrid + _dataService.Rows;
        private void UpdateCrownDescr() => _crownsDescription.text = StartCrowns + _dataService.Crowns;

        private void OpenCoinsClicked()
        {
            if (_dataService.CrownsAtMax || !_dataService.TryBuyGems(ChestCost)) return;
            _dataService.TryUpCrowns();
            UpdateCrownDescr();
            SetCrownsCost();
        }

        private void SetCrownsCost() => _coinsCost.text = _dataService.CrownsAtMax ? Max : ChestCost.ToString();
        private void SetRowsCost() => _gridCost.text = _dataService.RowsAtMax ? Max : GridUpCost().ToString();
        private void SetChestCost() => _chestCost.text = ChestCost.ToString();

        private void OpenGridClicked()
        {
            if (_dataService.RowsAtMax || !_dataService.TryBuyGems(GridUpCost())) return;
            _dataService.TryUpRows();
            UpdateRowsDescr();
            SetRowsCost();
        }

        private void OpenChestClicked()
        {
            List<(Race, Mastery)> closed = GetClosedTypes();
            SetChestCost();
            if (closed.Count == 0)
            {
                _chestResult.gameObject.SetActive(true);
                _chestResult.SetText(AllOpenText);
                _chestResult.SetMastery(null);
                _chestResult.SetRace(null);
                return;
            }
            
            if (!_dataService.TryBuyGems(ChestCost)) return;
            
            bool inTutor = !_dataService.IsOpened(Race.Human, Mastery.Ranger);
            (Race, Mastery) selection = inTutor ? (Race.Human, Mastery.Ranger) : GetNextType(closed);

            _dataService.SetOpened(selection);
            _chestResult.SetRace(_buffsDatabase.IconFor(selection.Item1));
            _chestResult.SetMastery(_buffsDatabase.IconFor(selection.Item2));

            string text = $"{_buffsDatabase.NameFor(selection.Item1)} {_buffsDatabase.NameFor(selection.Item2)}";
            _chestResult.SetText(text);
            _chestResult.gameObject.SetActive(true);
        }

        private (Race, Mastery) GetNextType(List<(Race, Mastery)> closed)
        {
            if (HasClosed(closed, Race.Human))
                return closed.Last(c => c.Item1 == Race.Human);
            if (HasClosed(closed, Race.Orc))
                return closed.Last(c => c.Item1 == Race.Orc);
            if (HasClosed(closed, Race.Undead))
                return closed.Last(c => c.Item1 == Race.Undead);
            if (HasClosed(closed, Race.Demon))
                return closed.Last(c => c.Item1 == Race.Demon);

            throw new Exception("All types opened");
        }
        
        private bool HasClosed(List<(Race, Mastery)> closed, Race race) => 
            closed.Where(c => c.Item1 == race).ToList().Count != 0;

        private List<(Race, Mastery)> GetClosedTypes()
        {
            Dictionary<Race,IEnumerable<Mastery>> allActorTypes = _unitsDatabase.AllActorTypes();
            var selectMany = allActorTypes.SelectMany(dict => dict.Value, (keyValuePair, mastery) => new {keyValuePair, mastery});
            var enumerable = selectMany.Where(t => !_dataService.IsOpened(t.mastery, t.keyValuePair.Key));
            IEnumerable<(Race Key, Mastery mastery)> valueTuples = enumerable.Select(t => (t.keyValuePair.Key, t.mastery));
            return valueTuples.ToList();
        }
        
        private int GridUpCost() => _dataService.Rows switch
        {
            1 => 100,
            2 => 300,
            3 => 500,
            _ => 99999
        };
        
        private string AllOpenText => YG2.lang switch
        {
            "ru" => "Все типы уже открыты.\r\nЖдите обновлений!",
            "tr" => "Tüm tipler halihazırda açık.\r\nGüncellemeleri bekleyin!",
            _ => "All types are already open.\r\nWait for updates!"
        };
        
        private string StartGrid => YG2.lang switch
        {
            "ru" => "Количество рядов: ",
            "tr" => "Satır sayısı:",
            _ => "Number of rows:"
        };
        
        private string StartCrowns => YG2.lang switch
        {
            "ru" => "Бонусные короны за победу: +",
            "tr" => "Kazananlara bonus taçlar: +",
            _ => "Bonus crowns for winning: +"
        };
    }
}