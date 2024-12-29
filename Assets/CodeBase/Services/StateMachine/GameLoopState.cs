using System.Linq;
using Gameplay.Units;
using Services.Buffs;
using Services.GridService;
using UI.UpgradeWindow;

namespace Services.StateMachine
{
    public class GameLoopState : IExitableState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly GridDataService _gridService;
        private readonly GameplayDataService _gameplayService;
        private readonly WaveBuilder _waveBuilder;
        private readonly BuffService _buffService;
        private readonly UpgradeDataService _upgradeDataService;

        private int _profit;
        private bool _isWin;

        public GameLoopState(GameStateMachine gameStateMachine,
                             GridDataService gridService, 
                             GameplayDataService gameplayService,
                             WaveBuilder waveBuilder,
                             BuffService buffService,
                             UpgradeDataService upgradeDataService)
        {
            _gameStateMachine = gameStateMachine;
            _gridService = gridService;
            _gameplayService = gameplayService;
            _waveBuilder = waveBuilder;
            _buffService = buffService;
            _upgradeDataService = upgradeDataService;
        }

        public void Enter()
        {
            _profit = _gameplayService.Gold;
            _gridService.Save();
            _buffService.ApplyBuffs(_gridService.PlayerUnits);
            _upgradeDataService.IncrementStats(_gridService.PlayerUnits);
            

            foreach (Actor actor in _gridService.PlayerUnits)
            {
                actor.Died += OnAllyDied;
                actor.Unleash();
            }

            foreach (Actor actor in _waveBuilder.EnemyUnits)
            {
                actor.Died += OnEnemyDied;
                actor.Unleash();
            }
        }

        public void Exit()
        {
            foreach (Actor actor in _waveBuilder.EnemyUnits)
                actor.Died -= OnEnemyDied;

            foreach (Actor actor in _gridService.PlayerUnits)
                actor.Died -= OnAllyDied;

            _profit = _gameplayService.Gold - _profit;
        }

        private void OnAllyDied()
        {
            if (_gridService.PlayerUnits.Any(a => !a.IsDead)) return;
            _isWin = false;
            _gameStateMachine.Enter<ResultScreenState, bool>(_isWin);
        }

        private void OnEnemyDied()
        {
            if (_waveBuilder.EnemyUnits.Any(a => !a.IsDead)) return;
            _isWin = true;
            _gameStateMachine.Enter<ResultScreenState, bool>(_isWin);
        }
    }
}