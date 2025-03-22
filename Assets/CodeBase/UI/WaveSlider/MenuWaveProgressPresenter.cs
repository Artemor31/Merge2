using Databases;
using Infrastructure;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.WaveSlider
{
    public class MenuWaveProgressPresenter : Presenter
    {
        [SerializeField] private TextMeshProUGUI _value;
        [SerializeField] private TextMeshProUGUI _currentMax;
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _rewardImage;
        [SerializeField] private TextMeshProUGUI _rewardAmount;
        [SerializeField] private TextMeshProUGUI _nextWaveValue;
        [SerializeField] private Sprite _coinBag;
        [SerializeField] private Sprite _gemBag;

        private WaveRewardsService _waveService;
        private int _nextValue;

        public override void Init()
        {
            _waveService = ServiceLocator.Resolve<WaveRewardsService>();
            _waveService.OnRewardCollected += HandleRewardCollected;
            Show();
        }

        private void HandleRewardCollected(int waveWithReward) => Show();
        private void Start() => Show();

        public void Show()
        {
            var waveData = _waveService.GetWaveViewData();
            
            CurrentRatio(waveData);
            SetView(waveData);
        }

        private void CurrentRatio((int CurrentWave, RewardData rewardData) waveData)
        {
            int min = waveData.rewardData.StartWave;
            int max = waveData.rewardData.WaveToGet;
            int current = waveData.CurrentWave;
            
            float maxNormal = max - min;
            float currentNormal = current - min;
            float f = currentNormal / maxNormal;
            int ratio = Mathf.RoundToInt(f * 100);
            _slider.value = f;
            _value.text = $"{ratio}%";
        }

        private void SetView((int CurrentWave, RewardData rewardData) waveData)
        {
            _currentMax.text = waveData.CurrentWave.ToString();
            _nextWaveValue.text = waveData.rewardData.WaveToGet.ToString();
            _rewardImage.sprite = waveData.rewardData.Type == Currency.Coin ? _coinBag : _gemBag;
            _rewardAmount.text = waveData.rewardData.Amount.ToString();
        }
    }
}