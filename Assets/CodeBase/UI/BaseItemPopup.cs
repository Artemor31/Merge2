using Infrastructure;
using Services.DataServices;
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
        
        protected PersistantDataService PersistantDataService;
        protected Currency Currency = Currency.None;
        public override void Init() => 
            PersistantDataService = ServiceLocator.Resolve<PersistantDataService>();
    }
}