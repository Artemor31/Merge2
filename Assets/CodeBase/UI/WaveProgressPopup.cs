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

        public void SetData(Currency currency, int amount)
        {
            _currency = currency;
            
            _coin.gameObject.SetActive(currency == Currency.Coin);
            _gem.gameObject.SetActive(currency == Currency.Gem);
            _coin.SetData(amount);
            _gem.SetData(amount);
            _bag.sprite = currency == Currency.Coin ? _coinBag : _gemBag;
            
            gameObject.SetActive(true);
        }
        
        private void CloseClicked() => gameObject.SetActive(false);
    }
}