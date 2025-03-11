using Databases;
using Infrastructure;
using UnityEngine.UI;

namespace UI
{
    public class WaveProgressPopup : BaseItemPopup
    {
        public Button OkButton => _ads;
        
        public override void Init()
        {
            base.Init();
            _ads.onClick.AddListener(CloseClicked);
            _close.onClick.AddListener(CloseClicked);
        }

        public void SetData(RewardData reward)
        {
            _currency = reward.Type;
            
            _coin.gameObject.SetActive(reward.Type == Currency.Coin);
            _gem.gameObject.SetActive(reward.Type == Currency.Gem);
            _coin.SetData(reward.Amount);
            _gem.SetData(reward.Amount);
            _bag.sprite = reward.Type == Currency.Coin ? _coinBag : _gemBag;
            
            gameObject.SetActive(true);
        }
        
        private void CloseClicked() => gameObject.SetActive(false);
    }
}