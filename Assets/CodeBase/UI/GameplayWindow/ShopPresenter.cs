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
        [SerializeField] private Button _buyUnit;
        [SerializeField] private Button _closeShop;
        [SerializeField] private TextMeshProUGUI _buyUnitCost;
        [SerializeField] private Button _nextUnit;
        [SerializeField] private Button _prevUnit;
        [SerializeField] private GameObject _unitShop;
        [SerializeField] private List<Image> _stars;
        [SerializeField] public Button _getUnitButton;

        private GridLogicService _gridService;
        private GameplayDataService _gameplayService;

        private int _selectedStars = 1;

        public override void Init()
        {
            _gridService = ServiceLocator.Resolve<GridLogicService>();
            _gameplayService = ServiceLocator.Resolve<GameplayDataService>();

            _closeShop.onClick.AddListener(CloseActorShop);
            _buyUnit.onClick.AddListener(TryBuyUnit);
            _nextUnit.onClick.AddListener(ClickNextUnit);
            _prevUnit.onClick.AddListener(ClickPrevUnit);
            _getUnitButton.onClick.AddListener(UnitForAdsRequested);
        }

        public override void OnShow()
        {
            UpdateCost();
            //if ()
            // show unit for ads button
        }

        private void UnitForAdsRequested()
        {
            if (_gridService.CanAddUnit() == false) return;
            _gridService.TryCreatePlayerUnit(_selectedStars);
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
                UpdateCost();
            }
        }

        private void CloseActorShop() => _unitShop.SetActive(false);

        private void UpdateCost() => _buyUnitCost.text = GetCostForLevel().ToString();
        
        // TODO move to config
        private int GetCostForLevel()
        {
            return 10 + _gameplayService.Wave * 3;
            switch (_selectedStars)
            {
                case 1:
                    return 10;
                case 2:
                    return 19;
                case 3:
                    return 26;
                default:
                    throw new Exception("not correct level");
            }
        }
    }
}