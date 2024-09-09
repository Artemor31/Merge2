using System.Linq;
using Gameplay.Units;
using Services.SaveService;

namespace Services.StateMachine
{
    public class GameLoopState : IExitableState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly GridDataService _gridService;
        private readonly PlayerProgressService _playerService;
        private readonly WaveBuilder _waveBuilder;
        
        private int _profit;
        private bool _isWin;

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
            _profit = _playerService.Money;

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

            _profit = _playerService.Money - _profit;
            
            _gameStateMachine.Enter<ResultScreenState, bool>(_isWin);
        }
        
        private void OnAllyDied()
        {
            if (_gridService.PlayerUnits.Any(a => !a.IsDead)) return;
            _isWin = false;
            Exit();
        }

        private void OnEnemyDied()
        {
            if (_waveBuilder.EnemyUnits.Any(a => !a.IsDead)) return;
            _isWin = true;
            Exit();
        }
    }
}