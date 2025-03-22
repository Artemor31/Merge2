using System.Text;
using Infrastructure;
using Services;
using TMPro;
using UnityEngine;

namespace UI.GameplayWindow
{
    public class BuffInfoPresenter : Presenter
    {
        [SerializeField] private TextMeshProUGUI _description;
        private BuffService _buffService;
        private GridService _gridLogicService;

        public override void Init()
        {
            _buffService = ServiceLocator.Resolve<BuffService>();
            _gridLogicService = ServiceLocator.Resolve<GridService>();
            _gridLogicService.OnPlayerFieldChanged += PlayerFieldChanged;
        }

        public override void OnShow() => _description.text = CreteDescription();

        private void PlayerFieldChanged()
        {
            if (gameObject.activeInHierarchy)
            {
                _description.text = CreteDescription();
            }
        }

        private string CreteDescription()
        {
            StringBuilder stringBuilder = new();
            var buffs = _buffService.CalculateBuffs(_gridLogicService.PlayerUnits);
            foreach (string buff in buffs)
            {
                stringBuilder.Append(buff);
                stringBuilder.Append("\r\n");
            }
            
            return stringBuilder.ToString();
        }
    }
}