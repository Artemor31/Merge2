using Services.BuffService;
using Infrastructure;
using UnityEngine;
using TMPro;

namespace UI.MenuWindow
{
    public class BuffInfoPresenter : Presenter
    {
        [SerializeField] private TextMeshProUGUI _description;
        private BuffViewService _buffService;

        public override void Init()
        {
            _buffService = ServiceLocator.Resolve<BuffViewService>();
        }
    }
}