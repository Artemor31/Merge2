using Infrastructure;

namespace UI
{
    public class WaveProgressPopup : BaseItemPopup
    {
        private int _amount;

        public override void Init()
        {
            base.Init();
            _ads.onClick.AddListener(AdsClicked);
            _close.onClick.AddListener(CloseClicked);
        }

        public void SetData(Currency currency, int amount)
        {
            _currency = currency;
            _amount = amount;

            if (currency == Currency.Coin)
            {
                _bag.sprite = _coinBag;
                _coin.gameObject.SetActive(true);
                _gem.gameObject.SetActive(false);
            }
            else if (currency == Currency.Gem)
            {
                _bag.sprite = _gemBag;
                _coin.gameObject.SetActive(false);
                _gem.gameObject.SetActive(true);
            }
        }
        
        private void CloseClicked() => gameObject.SetActive(false);

        private void AdsClicked()
        {
            switch (_currency)
            {
                case Currency.Coin:
                    _persistantDataService.AddCoins(_amount);
                    break;
                case Currency.Gem:
                    _persistantDataService.AddGems(_amount);
                    break;
            }
        }
    }
}