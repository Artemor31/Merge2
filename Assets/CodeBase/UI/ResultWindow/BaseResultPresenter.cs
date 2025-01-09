using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Services;
using Services.SaveProgress;
using Services.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI.ResultWindow
{
    public class ResultData : WindowData
    {
        public int CrownsValue;
        public int CoinsValue;
        public int GemsValue;
    }

    public abstract class BaseResultPresenter : Presenter
    {
        [SerializeField] private Transform _rewardParent;
        [SerializeField] private CurrencyElement _prefab;
        [SerializeField] private CurrencyPair[] _pairs;
        [SerializeField] private Button _nextLevel;
        [SerializeField] private Button _showAds;

        protected GameStateMachine GameStateMachine;
        protected ResultData ResultData;
        
        private PersistantDataService _persistantDataService;
        private List<CurrencyElement> _rewards;
        private GameplayDataService _gameplayDataService;

        public override void Init()
        {
            _persistantDataService = ServiceLocator.Resolve<PersistantDataService>();
            _gameplayDataService = ServiceLocator.Resolve<GameplayDataService>();
            GameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _nextLevel.onClick.AddListener(OnNextLevelClicked);
            _showAds.onClick.AddListener(OnShowAdsClicked);
            _rewards = new List<CurrencyElement>();
        }

        protected virtual void OnNextLevelClicked()
        {
            _persistantDataService.AddCoins(ResultData.CoinsValue);
            _persistantDataService.AddGems(ResultData.GemsValue);
            _gameplayDataService.AddCrowns(ResultData.CrownsValue);
            gameObject.SetActive(false);
        }

        protected void AddReward(Currency currency, string value)
        {
            CurrencyElement element = Instantiate(_prefab, _rewardParent);
            element.SetData(SpriteFor(currency), value);
            _rewards.Add(element);
        }

        protected void Clear()
        {
            foreach (CurrencyElement element in _rewards)
            {
                Destroy(element.gameObject);
            }

            _rewards.Clear();
            ResultData = new ResultData();
        }

        private Sprite SpriteFor(Currency currency) => _pairs.First(p => p.Currency == currency).Sprite;
        private void OnShowAdsClicked() => YG2.RewardedAdvShow(AdsId.DoubleReward, AdWatched);

        private void AdWatched()
        {
            _persistantDataService.AddCoins(ResultData.CoinsValue);
            _persistantDataService.AddGems(ResultData.GemsValue);
            _gameplayDataService.AddCrowns(ResultData.CrownsValue);
            OnNextLevelClicked();
        }

        [Serializable]
        public class CurrencyPair
        {
            public Sprite Sprite;
            public Currency Currency;
        }
    }
}