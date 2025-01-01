using System.Collections.Generic;
using System.Text;
using Databases;
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
        private GridDataService _gridDataService;

        public override void Init()
        {
            _buffService = ServiceLocator.Resolve<BuffService>();
            _gridLogicService = ServiceLocator.Resolve<GridLogicService>();
            _gridDataService = ServiceLocator.Resolve<GridDataService>();
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
            List<BuffConfig> buffs = _buffService.CalculateBuffs(_gridDataService.PlayerUnits);
            foreach (BuffConfig buff in buffs)
            {
                stringBuilder.Append(buff.Description);
                stringBuilder.Append("\r\n");
            }
            
            return stringBuilder.ToString();
        }
    }
}