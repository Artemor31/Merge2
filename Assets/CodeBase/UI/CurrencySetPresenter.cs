using Infrastructure;
using Services.DataServices;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CurrencySetPresenter : Presenter
    {
        [SerializeField] private TextMeshProUGUI _softText;
        [SerializeField] private TextMeshProUGUI _hardText;
        private PersistantDataService _persistantDataService;

        public override void Init()
        {
            _persistantDataService = ServiceLocator.Resolve<PersistantDataService>();
            _persistantDataService.OnCoinsChanged += HandleCoinsChanged;
            _persistantDataService.OnGemsChanged += HandleGemsChanged;
            HandleCoinsChanged(_persistantDataService.Coins);
            HandleGemsChanged(_persistantDataService.Gems);
        }
        
        private void HandleCoinsChanged(int value) => _softText.text = _persistantDataService.Coins.ToString();
        private void HandleGemsChanged(int value) => _hardText.text = _persistantDataService.Gems.ToString();
    }
}