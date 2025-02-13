using Infrastructure;
using Services;
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
        private int _savedRecord;
        private int _savedNext;

        public override void Init()
        {
            _persistantDataService = ServiceLocator.Resolve<PersistantDataService>();
            _waveProgressPopup.OkButton.onClick.AddListener(UpdateView);
            _savedRecord = _persistantDataService.MaxWave;
            _savedNext = _persistantDataService.NextWave;
        }

        public void Show()
        {
            int record = _persistantDataService.MaxWave;
            int nextWave = _persistantDataService.NextWave;
            
            _currentMax.text = record.ToString();
            _nextWaveValue.text = nextWave.ToString();
            
            FillSlider(record, nextWave);
            
            var reward = _persistantDataService.RewardCurrentReward();
            SetReward(reward);

            if (record > _savedRecord && nextWave > _savedNext)
            {
                _waveProgressPopup.SetData(reward.Currency, reward.Amount);
            }
        }
        
        private void UpdateView()
        {
            _savedRecord = _persistantDataService.MaxWave;
            _savedNext = _persistantDataService.NextWave;
            Show();
        }

        private void SetReward((Currency Currency, int Amount) reward)
        {
            _rewardImage.sprite =  reward.Currency == Currency.Coin ? _coinBag : _gemBag;
            _rewardAmount.text = reward.Amount.ToString();
        }

        private void FillSlider(int current, int max)
        {
            float currentFloat = current;
            float f = currentFloat / max;
            _slider.value = f;
            int ratio = Mathf.RoundToInt(f * 100);
            _value.text = $"{ratio}%";
        }
    }
}