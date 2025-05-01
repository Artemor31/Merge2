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
            
            //_openChest.onClick.AddListener(OpenChestClicked);
            _buyGrid.onClick.AddListener(OpenGridClicked);
            _buyCoins.onClick.AddListener(OpenCoinsClicked);
            SetChestCost();
           // SetCrownsCost();
          //  SetRowsCost();
           // UpdateCrownDescr();
           // UpdateRowsDescr();
        }

        public override void OnShow() => YG2.InterstitialAdvShow();
       // private void UpdateRowsDescr() => _gridDescription.text = StartGrid + _dataService.Rows;
        //private void UpdateCrownDescr() => _crownsDescription.text = StartCrowns + _dataService.Crowns;

        private void OpenCoinsClicked()
        {
        //    if (_dataService.CrownsAtMax || !_dataService.TryBuyGems(ChestCost)) return;
         //   _dataService.TryUpCrowns();
          //  UpdateCrownDescr();
        //    SetCrownsCost();
        }

       // private void SetCrownsCost() => _coinsCost.text = _dataService.CrownsAtMax ? Max : ChestCost.ToString();
       // private void SetRowsCost() => _gridCost.text = _dataService.RowsAtMax ? Max : GridUpCost().ToString();
        private void SetChestCost() => _chestCost.text = ChestCost.ToString();

        private void OpenGridClicked()
        {
           // if (_dataService.RowsAtMax || !_dataService.TryBuyGems(GridUpCost())) return;
          //  _dataService.TryUpRows();
          ///  UpdateRowsDescr();
        //    SetRowsCost();
        }

        

        
        // private int GridUpCost() => _dataService.Rows switch
        // {
        //     1 => 100,
        //     2 => 300,
        //     3 => 500,
        //     _ => 99999
        // };
        
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