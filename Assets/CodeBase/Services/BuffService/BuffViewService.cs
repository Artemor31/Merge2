using System;
using System.Text;
using Services.GridService;
using Services.Infrastructure;
using Services.StateMachine;

namespace Services.BuffService
{
    public class BuffViewService : IService
    {
        public event Action OnBuffsChanged;
        
        private readonly BuffService _buffService;
        private readonly GridLogicService _gridLogicService;
        private readonly GameStateMachine _stateMachine;
        private readonly GridDataService _gridDataService;

        public BuffViewService(BuffService buffService, 
            GridLogicService gridLogicService,
            GameStateMachine stateMachine,
            GridDataService gridDataService)
        {
            _buffService = buffService;
            _gridLogicService = gridLogicService;
            _stateMachine = stateMachine;
            _gridDataService = gridDataService;

            _stateMachine.OnStateChanged += StateChanged;
            _gridLogicService.OnPlayerFieldChanged += PlayerFieldChanged;
        }

        public string CreteDescription()
        {
            StringBuilder stringBuilder = new();
            var units = _gridDataService.PlayerUnits;
            var buffs = _buffService.CalculateBuffs(units);
            foreach (var buff in buffs)
            {
                stringBuilder.Append(buff.Description);
                stringBuilder.Append("\r\n");
            }
            
            return stringBuilder.ToString();
        }

        private void PlayerFieldChanged() => OnBuffsChanged?.Invoke();

        private void StateChanged(IState newState)
        {
            if (newState.GetType() == typeof(SetupLevelState))
            {
                OnBuffsChanged?.Invoke();
            }

            if (newState.GetType() == typeof(GameLoopState))
            {
                _buffService.ApplyBuffs(_gridDataService.PlayerUnits);
            }
        }
    }
}