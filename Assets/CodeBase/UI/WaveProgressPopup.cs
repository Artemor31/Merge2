using Databases;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WaveProgressPopup : Presenter, IWindowDataReceiver<RewardData>
    {
        [SerializeField] protected CurrencyElement _coin;
        [SerializeField] protected CurrencyElement _gem;
        [SerializeField] private Button _ok;
        [SerializeField] protected Sprite _coinBag;
        [SerializeField] protected Sprite _gemBag;
        [SerializeField] protected Image _bag;
        
        public override void Init() => _ok.onClick.AddListener(() => gameObject.SetActive(false));

        public void SetData(RewardData reward)
        {
            _coin.gameObject.SetActive(reward.Type == Currency.Coin);
            _gem.gameObject.SetActive(reward.Type == Currency.Gem);
            _coin.SetData(reward.Amount);
            _gem.SetData(reward.Amount);
            _bag.sprite = reward.Type == Currency.Coin ? _coinBag : _gemBag;
            
            gameObject.SetActive(true);
        }
    }
}