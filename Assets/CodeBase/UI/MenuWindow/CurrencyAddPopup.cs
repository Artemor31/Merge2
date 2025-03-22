using Infrastructure;
using Services.Infrastructure;
using YG;

namespace UI.MenuWindow
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
            Currency = currency;
            _coin.gameObject.SetActive(currency == Currency.Coin);
            _gem.gameObject.SetActive(currency == Currency.Gem);
            _bag.sprite = currency == Currency.Coin ? _coinBag : _gemBag;
        }
        
        private void CloseClicked() => gameObject.SetActive(false);
        private void AdsClicked() => YG2.RewardedAdvShow(AdsId.SimpleAddCurrency, Callback);
        
        private void Callback()
        {
            switch (Currency)
            {
                case Currency.None: return;
                case Currency.Coin: PersistantDataService.AddCoins(100);
                    break;
                case Currency.Gem: PersistantDataService.AddGems(30);
                    break;
            }

            Currency = Currency.None;
            CloseClicked();
        }
    }
}