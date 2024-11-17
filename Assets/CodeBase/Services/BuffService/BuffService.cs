using System;
using Databases;
using System.Linq;
using Gameplay.Units;
using Services.SaveService;
using Services.StateMachine;
using System.Collections.Generic;

namespace Services.BuffService
{

    public class BuffViewService : IService
    {
        public event Action OnBuffsChanged;
        
        private readonly BuffService _buffService;
        private readonly GridLogicService _gridService;
        private readonly GameStateMachine _stateMachine;

        public BuffViewService(BuffService buffService, GridLogicService gridService, GameStateMachine stateMachine)
        {
            _buffService = buffService;
            _gridService = gridService;
            _stateMachine = stateMachine;

            _stateMachine.OnStateChanged += StateChanged;
            _gridService.OnPlayerFieldChanged += PlayerFieldChanged;
        }

        private void PlayerFieldChanged()
        {
            OnBuffsChanged?.Invoke();
        }

        private void StateChanged(IState newState)
        {
            if (newState.GetType() != typeof(GameLoopState)) return;
            
            OnBuffsChanged?.Invoke();
        }
    }
    
    public class BuffService : IService
    {
        public List<string> ActiveDescriptions = new();

        private readonly List<BuffAction> _actions;
        private readonly List<Race> _races;
        private readonly IEnumerable<Mastery> _masteries;

        public BuffService()
        {
            _actions = new List<BuffAction>();
            foreach (var type in GetAllBuffActions())
            {
                _actions.Add(Activator.CreateInstance(type) as BuffAction);
            }

            _races = Enum.GetValues(typeof(Race)).Cast<Race>().Skip(1).ToList();
            _masteries = Enum.GetValues(typeof(Mastery)).Cast<Mastery>().Skip(1);
        }

        private static IEnumerable<Type> GetAllBuffActions()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(BuffAction)));
        }

        public void CalculateBuffs(List<Actor> actors)
        {
            ActiveDescriptions.Clear();

            foreach (var mastery in _masteries)
            {
                if (actors.All(a => a.Data.Mastery != mastery)) continue;
                {
                    var buff = _actions.First(a => a.Mastery == mastery);
                    ActiveDescriptions.Add(buff.Description);
                }
            }

            foreach (var race in _races)
            {
                if (actors.All(a => a.Data.Race != race)) continue;
                
                var buff = _actions.First(a => a.Race == race);
                ActiveDescriptions.Add(buff.Description);
            }
        }

        public void ApplyBuffs(List<Actor> actors)
        {
        }
    }
}