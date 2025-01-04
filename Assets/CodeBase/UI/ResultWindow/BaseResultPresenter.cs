using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Services.StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ResultWindow
{
    public abstract class BaseResultPresenter : Presenter
    {
        [SerializeField] private Transform _rewardParent;
        [SerializeField] private CurrencyElement _prefab;
        [SerializeField] private CurrencyPair[] _pairs;
        [SerializeField] private Button _nextLevel;
        [SerializeField] private Button _showAds;

        private GameStateMachine _gameStateMachine;
        private List<CurrencyElement> _rewards;

        public override void Init()
        {
            _rewards = new List<CurrencyElement>();
            _gameStateMachine = ServiceLocator.Resolve<GameStateMachine>();
            _nextLevel.onClick.AddListener(OnNextLevelClicked);
        }

        public void AddReward(Currency currency, string value)
        {
            CurrencyElement element = Instantiate(_prefab, _rewardParent);
            element.SetData(SpriteFor(currency), value);
            _rewards.Add(element);
        }

        public void Clear()
        {
            foreach (CurrencyElement element in _rewards)
            {
                Destroy(element.gameObject);
            }
            
            _rewards.Clear();
        }

        private Sprite SpriteFor(Currency currency) => _pairs.First(p => p.Currency == currency).Sprite;

        private void OnNextLevelClicked()
        {
            gameObject.SetActive(false);
            _gameStateMachine.Enter<LoadLevelState>();
        }

        [Serializable]
        public class CurrencyPair
        {
            public Sprite Sprite;
            public Currency Currency;
        }
    }
}