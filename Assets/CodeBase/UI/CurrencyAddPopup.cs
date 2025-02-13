using System;
using Infrastructure;
using Services.SaveProgress;
using YG;

namespace UI
{
    public class CurrencyAddPopup : BaseItemPopup
    {
        public override void Init()
        {
            base.Init();
            _ads.onClick.AddListener(AdsClicked);
            _close.onClick.AddListener(CloseClicked);
            _coin.SetData(100);
            _gem.SetData(30);
        }
        
        public void ShowData(Currency currency)
        {
            gameObject.SetActive(true);
            _currency = currency;
            _coin.gameObject.SetActive(currency == Currency.Coin);
            _gem.gameObject.SetActive(currency == Currency.Gem);
            _bag.sprite = currency == Currency.Coin ? _coinBag : _gemBag;
        }
        
        private void CloseClicked() => gameObject.SetActive(false);
        private void AdsClicked() => YG2.RewardedAdvShow(AdsId.SimpleAddCurrency, Callback);
        
        private void Callback()
        {
            switch (_currency)
            {
                case Currency.None: return;
                case Currency.Coin: _persistantDataService.AddCoins(100);
                    break;
                case Currency.Gem: _persistantDataService.AddGems(30);
                    break;
            }

            _currency = Currency.None;
            CloseClicked();
        }
    }
}