using System.Collections.Generic;
using Infrastructure;
using Services;
using Services.GridService;
using Services.SaveProgress;
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
        [SerializeField] public Image _getUnitButtonImage;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _secondColor;

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
        
        private void Update()
        {
            float t = Mathf.PingPong(Time.time / 1, 1);
            _getUnitButtonImage.color = Color.Lerp(_startColor, _secondColor, t);
        }

        public override void OnShow()
        {
            UpdateCost();
            _getUnitButton.gameObject.SetActive(_gameplayService.Wave % 3 == 0 && _gameplayService.Wave > 1);
        }

        private void UnitForAdsRequested()
        {
            if (_gridService.CanAddUnit() == false) return;
            YG2.RewardedAdvShow(AdsId.GetUnit, GetUnitForAds);
        }

        private void GetUnitForAds()
        {
            _gridService.TryCreatePlayerUnit(_selectedStars);
            _getUnitButton.gameObject.SetActive(false);
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
            if (_gameplayService.TryBuy(CostOfUnit()))
            {
                _gridService.TryCreatePlayerUnit(_selectedStars);
                UpdateCost();
            }
        }

        private void CloseActorShop() => _unitShop.SetActive(false);
        private void UpdateCost() => _buyUnitCost.text = CostOfUnit().ToString();
        private int CostOfUnit() => 10 + _gameplayService.Wave * 3;
    }
}