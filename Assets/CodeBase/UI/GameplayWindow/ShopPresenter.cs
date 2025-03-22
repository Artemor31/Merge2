using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using Services;
using Services.DataServices;
using Services.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

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

        private GridService _gridService;
        private GameplayDataService _gameplayService;

        private int _selectedStars = 1;
        private bool _adsRequested;

        public override void Init()
        {
            _gridService = ServiceLocator.Resolve<GridService>();
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
            _adsRequested = false;
        }

        private void UnitForAdsRequested()
        {
            if (_gridService.CanAddUnit == false) return;
            YG2.RewardedAdvShow(AdsId.GetUnit, GetUnitForAds);
        }

        private void GetUnitForAds()
        {
            _gridService.TryCreatePlayerUnit(_selectedStars);
            _getUnitButton.gameObject.SetActive(false);
            _adsRequested = true;
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
            if (_gridService.CanAddUnit == false) return;
            if (_gameplayService.TryBuy(CostOfUnit()))
            {
                _gridService.TryCreatePlayerUnit(_selectedStars);
                UpdateCost();

                if (_gameplayService.Crowns < CostOfUnit() && !_adsRequested)
                {
                    StartCoroutine(ShowGetUnitForAds());
                }
            }
        }

        private IEnumerator ShowGetUnitForAds()
        {
            yield return new WaitForSeconds(1);
            _getUnitButton.gameObject.SetActive(true);
        }

        private void CloseActorShop() => _unitShop.SetActive(false);
        private void UpdateCost() => _buyUnitCost.text = CostOfUnit().ToString();
        private int CostOfUnit() => 10 + _gameplayService.Wave * 3;
    }
}