using Infrastructure;
using Services.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameplayWindow
{
    public class ChestOpenPresenter : Presenter
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private TextMeshProUGUI _resultText;
        private RewardsService _rewardsService;


        public override void Init()
        {
            _rewardsService = ServiceLocator.Resolve<RewardsService>();
        }

        private void OpenChest()
        {
            if (Random.Range(1, 4) == 1)
            {
                _resultText.text = _rewardsService.OpenRandomUnit() ??
                                   _rewardsService.GetChestGold().ToString();
            }
        }
    }
}