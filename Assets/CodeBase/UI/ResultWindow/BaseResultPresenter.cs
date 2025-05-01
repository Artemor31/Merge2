using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Infrastructure;
using Services.DataServices;
using Services.Infrastructure;
using Services.StateMachine;
using TMPro;
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
        public int KeysValue;
    }

    public abstract class BaseResultPresenter : Presenter
    {
        [SerializeField] private CurrencyDatabase _currencyDatabase;
        [SerializeField] private TextMeshProUGUI _header;
        [SerializeField] private Transform _rewardParent;
        [SerializeField] private CurrencyElement _prefab;
        [SerializeField] private Button _nextLevel;
        [SerializeField] private Button _showAds;

        protected GameStateMachine GameStateMachine;
        private PersistantDataService _persistantDataService;
        private List<CurrencyElement> _rewards;
        private GameplayDataService _gameplayDataService;
        private ResultData _resultData;

        public override void Init()
        {
            _persistantDataService = ServiceLocator.Resolve<PersistantDataService>();
            _gameplayDataService = ServiceLocator.Resolve<GameplayDataService>();
            GameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _nextLevel.onClick.AddListener(OnNextLevelClicked);
            if (_showAds)
            {
                _showAds.onClick.AddListener(OnShowAdsClicked);
            }

            _rewards = new List<CurrencyElement>();
        }

        public void SetData(ResultData data)
        {
            Clear();
            _resultData = data;

            AddReward(Currency.Crown, _resultData.CrownsValue);
            AddReward(Currency.Coin, _resultData.CoinsValue);
            AddReward(Currency.Gem, _resultData.GemsValue);
            AddReward(Currency.Key, _resultData.KeysValue);
            _header.text = GetHeader(_gameplayDataService.Wave);
        }

        protected abstract string GetHeader(int level);

        protected virtual void OnNextLevelClicked()
        {
            gameObject.SetActive(false);

            if (_gameplayDataService.Wave % 2 == 0)
            {
                YG2.InterstitialAdvShow();
            }
        }

        private void AddReward(Currency currency, int value)
        {
            if (value <= 0) return;

            CurrencyElement element = Instantiate(_prefab, _rewardParent);
            element.SetData(SpriteFor(currency), value.ToString());
            _rewards.Add(element);
        }

        private void Clear()
        {
            foreach (CurrencyElement element in _rewards)
            {
                Destroy(element.gameObject);
            }

            _rewards.Clear();
            _resultData = new ResultData();
        }

        private Sprite SpriteFor(Currency currency)
        {
            return _currencyDatabase.CurrencyDatas.First(c => c.Type == currency).Sprite;
        }

        private void OnShowAdsClicked() => YG2.RewardedAdvShow(AdsId.DoubleReward, AdWatched);

        private void AdWatched()
        {
            _persistantDataService.AddCoins(_resultData.CoinsValue);
            _persistantDataService.AddGems(_resultData.GemsValue);
            _gameplayDataService.AddCrowns(_resultData.CrownsValue);
            OnNextLevelClicked();
        }
    }
}