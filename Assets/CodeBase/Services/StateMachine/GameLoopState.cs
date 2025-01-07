using System.Linq;
using Gameplay.Units;
using Services.Buffs;
using Services.GridService;
using Services.Infrastructure;
using UI;
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
        private readonly WindowsService _windowsService;

        private int _profit;

        public GameLoopState(GameStateMachine gameStateMachine,
                             GridDataService gridService, 
                             GameplayDataService gameplayService,
                             WaveBuilder waveBuilder,
                             BuffService buffService,
                             UpgradeDataService upgradeDataService,
                             WindowsService windowsService)
        {
            _gameStateMachine = gameStateMachine;
            _gridService = gridService;
            _gameplayService = gameplayService;
            _waveBuilder = waveBuilder;
            _buffService = buffService;
            _upgradeDataService = upgradeDataService;
            _windowsService = windowsService;
        }

        public void Enter()
        {
            if (_gridService.PlayerUnits.Count == 0)
            {
                _gameStateMachine.Enter<ResultScreenState, bool>(false);
                return;
            }
            
            _profit = _gameplayService.Crowns;
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
            
            _windowsService.Show<GameCanvas>();
        }

        public void Exit()
        {
            foreach (Actor actor in _waveBuilder.EnemyUnits)
            {
                actor.Died -= OnEnemyDied;
                actor.Dispose();
            }

            foreach (Actor actor in _gridService.PlayerUnits)
            {
                actor.Died -= OnAllyDied;
                actor.Dispose();
            }

            _profit = _gameplayService.Crowns - _profit;
            _windowsService.Close<GameCanvas>();
        }

        private void OnAllyDied()
        {
            if (_gridService.PlayerUnits.All(a => a.IsDead))
            {
                _gameStateMachine.Enter<ResultScreenState, bool>(false);
            }
        }

        private void OnEnemyDied()
        {
            if (_waveBuilder.EnemyUnits.All(a => a.IsDead))
            {
                _gameStateMachine.Enter<ResultScreenState, bool>(true);
            }
        }
    }
}