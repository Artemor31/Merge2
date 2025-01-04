using System;
using System.Collections.Generic;
using Databases;
using Infrastructure;
using Services;
using Services.GridService;
using Services.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class ShopPresenter : Presenter
    {
        [SerializeField] private Button _openUnitShop;
        [SerializeField] private Button _buyUnit;
        [SerializeField] private Button _closeShop;
        [SerializeField] private TextMeshProUGUI _buyUnitCost;
        [SerializeField] private Button _nextUnit;
        [SerializeField] private Button _prevUnit;
        [SerializeField] private GameObject _unitShop;
        [SerializeField] private List<Image> _stars;

        private UnitsDatabase _unitsDatabase;
        private GridLogicService _gridService;
        private GameplayDataService _gameplayService;

        private int _selectedStars;

        public override void Init()
        {
            _unitsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<UnitsDatabase>();
            _gridService = ServiceLocator.Resolve<GridLogicService>();
            _gameplayService = ServiceLocator.Resolve<GameplayDataService>();

            _openUnitShop.onClick.AddListener(OpenActorShop);
            _closeShop.onClick.AddListener(CloseActorShop);
            _buyUnit.onClick.AddListener(TryBuyUnit);
            _nextUnit.onClick.AddListener(ClickNextUnit);
            _prevUnit.onClick.AddListener(ClickPrevUnit);
        }

        private void ClickPrevUnit()
        {
            if (_selectedStars == 1) return;

            _selectedStars--;
            _stars[_selectedStars].color = Color.black;
            UpdateCost();
        }

        private void ClickNextUnit()
        {
            if (_selectedStars == _stars.Count) return;

            _selectedStars++;
            _stars[_selectedStars - 1].color = Color.white;
            UpdateCost();
        }

        private void TryBuyUnit()
        {
            if (_gridService.CanAddUnit() == false) return;
            if (_gameplayService.TryBuy(GetCostForLevel()))
            {
                _gridService.TryCreatePlayerUnit(_selectedStars);
            }
        }

        private void CloseActorShop() => _unitShop.SetActive(false);

        private void OpenActorShop()
        {
            _selectedStars = 1;
            TryBuyUnit();
            return;
            _unitShop.SetActive(true);
            _stars.ForEach(s => s.color = Color.black);
            _stars[0].color = Color.white;
            _selectedStars = 1;
            UpdateCost();
        }

        private void UpdateCost() => _buyUnitCost.text = GetCostForLevel().ToString();
        
        // TODO move to config
        private int GetCostForLevel() => _selectedStars switch
        {
            1 => 10,
            2 => 19,
            3 => 26,
            _ => throw new Exception("not correct level")
        };
    }
}