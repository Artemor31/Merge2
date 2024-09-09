using System.Linq;
using Gameplay.Units;
using Services.SaveService;

namespace Services.StateMachine
{
    public class GameLoopState : IExitableState
    {     
        public bool IsWin { get; private set; }
        private int Profit { get; set; }

        private readonly GameStateMachine _gameStateMachine;
        private readonly GridDataService _gridService;
        private readonly PlayerProgressService _playerService;
        private readonly WaveBuilder _waveBuilder;

        public GameLoopState(GameStateMachine gameStateMachine,
                             GridDataService gridService, 
                             PlayerProgressService playerService,
                             WaveBuilder waveBuilder)
        {
            _gameStateMachine = gameStateMachine;
            _gridService = gridService;
            _playerService = playerService;
            _waveBuilder = waveBuilder;
        }

        public void Enter()
        {
            _gridService.Save();
            Profit = _playerService.Money;

            foreach (Actor actor in _gridService.PlayerUnits)
            {
                actor.Died += OnAllyDied;
                actor.Unleash(_waveBuilder.EnemyUnits);
            }

            foreach (Actor actor in _waveBuilder.EnemyUnits)
            {
                actor.Died += OnEnemyDied;
                actor.Unleash(_gridService.PlayerUnits);
            }
        }

        public void Exit()
        {
            foreach (Actor actor in _waveBuilder.EnemyUnits)
                actor.Died -= OnEnemyDied;

            foreach (Actor actor in _gridService.PlayerUnits)
                actor.Died -= OnAllyDied;

            Profit = _playerService.Money - Profit;
            
            _gameStateMachine.Enter<ResultScreenState>();
        }
        
        private void OnAllyDied()
        {
            if (_gridService.PlayerUnits.Any(a => !a.IsDead)) return;
            IsWin = false;
            Exit();
        }

        private void OnEnemyDied()
        {
            if (_waveBuilder.EnemyUnits.Any(a => !a.IsDead)) return;
            IsWin = true;
            Exit();
        }
    }
}