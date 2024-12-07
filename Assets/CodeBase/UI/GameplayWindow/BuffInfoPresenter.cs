using Infrastructure;
using Services.BuffService;
using TMPro;
using UnityEngine;

namespace UI.GameplayWindow
{
    public class BuffInfoPresenter : Presenter
    {
        [SerializeField] private TextMeshProUGUI _description;
        private BuffViewService _buffService;

        public override void Init()
        {
            _buffService = ServiceLocator.Resolve<BuffViewService>();
            _buffService.OnBuffsChanged += RewriteDescription;
        }

        private void RewriteDescription() => _description.text = _buffService.CreteDescription();
    }
}