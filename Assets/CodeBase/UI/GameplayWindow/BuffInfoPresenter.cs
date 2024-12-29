using Infrastructure;
using Services.Buffs;
using Services.GridService;
using TMPro;
using UnityEngine;

namespace UI.GameplayWindow
{
    public class BuffInfoPresenter : Presenter
    {
        [SerializeField] private TextMeshProUGUI _description;
        private BuffService _buffService;
        private GridLogicService _gridLogicService;

        public override void Init()
        {
            _buffService = ServiceLocator.Resolve<BuffService>();
            _gridLogicService = ServiceLocator.Resolve<GridLogicService>();
            _gridLogicService.OnPlayerFieldChanged += PlayerFieldChanged;
        }

        private void PlayerFieldChanged() => _description.text = _buffService.CreteDescription();
    }
}