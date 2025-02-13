using Infrastructure;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BaseItemPopup : Presenter
    {
        [SerializeField] protected CurrencyElement _coin;
        [SerializeField] protected CurrencyElement _gem;
        [SerializeField] protected Button _ads;
        [SerializeField] protected Button _close;
        [SerializeField] protected Sprite _coinBag;
        [SerializeField] protected Sprite _gemBag;
        [SerializeField] protected Image _bag;
        
        protected PersistantDataService _persistantDataService;
        protected Currency _currency = Currency.None;

        public override void Init()
        {
            _persistantDataService = ServiceLocator.Resolve<PersistantDataService>();
        }
    }
}