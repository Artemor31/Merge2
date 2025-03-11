using System;
using Databases;
using Infrastructure;
using Services;
using Services.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuWaveProgressPresenter : Presenter
    {
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private TextMeshProUGUI _currentMax;
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _rewardAmount;
        [SerializeField] private TextMeshProUGUI _nextWaveValue;
        [SerializeField] private WaveProgressPopup _waveProgressPopup;
        [SerializeField] private Sprite _coinBag;
        [SerializeField] private Sprite _gemBag;

        private PersistantDataService _persistantDataService;
        private int _nextValue;
        private GameplayDataService _gameplayDataService;
        private WaveRewardsDatabase _waveRewardsDatabase;

        public override void Init()
        {
            _persistantDataService = ServiceLocator.Resolve<PersistantDataService>();
            _gameplayDataService = ServiceLocator.Resolve<GameplayDataService>();
            _waveRewardsDatabase = ServiceLocator.Resolve<DatabaseProvider>().GetDatabase<WaveRewardsDatabase>();
            _waveProgressPopup.OkButton.onClick.AddListener(Show);
            Show();
        }

        private void Start() => Show();

        public void Show()
        {
            int current = _persistantDataService.MaxWave;
            var rewardData = _waveRewardsDatabase.GetFor(current);

            _nextValue = rewardData.WaveToGet;

            FillSlider(current, rewardData.StartWave, rewardData.WaveToGet);
            SetView(current, rewardData);

            if (rewardData.WaveToGet > _nextValue)
            {
                _waveProgressPopup.SetData(rewardData);

                switch (rewardData.Type)
                {
                    case Currency.Coin:
                        _persistantDataService.AddCoins(rewardData.Amount);
                        break;
                    case Currency.Gem:
                        _persistantDataService.AddGems(rewardData.Amount);
                        break;
                }
            }
        }

        private void SetView(int current, RewardData rewardData)
        {
            _currentMax.text = current.ToString();
            _nextWaveValue.text = _nextValue.ToString();
            _rewardImage.sprite = rewardData.Type == Currency.Coin ? _coinBag : _gemBag;
            _rewardAmount.text = rewardData.Amount.ToString();
        }

        private void FillSlider(int current,int min, int max)
        {
            float maxNormal = max - min;
            float currentNormal = current - min;
            float f = currentNormal / maxNormal;
            _slider.value = f;
            int ratio = Mathf.RoundToInt(f * 100);
            _value.text = $"{ratio}%";
        }
    }
}