using System.Collections.Generic;
using System.Linq;
using Gameplay.Units;
using Services.Buffs;
using Services.GridServices;
using Services.Infrastructure;
using UI.GameplayWindow;
using UI.UpgradeWindow;

namespace Services.StateMachine
{
    public class GameLoopState : IExitableState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly GridDataService _gridDataService;
        private readonly GameplayDataService _gameplayService;
        private readonly WaveBuilder _waveBuilder;
        private readonly BuffService _buffService;
        private readonly UpgradeDataService _upgradeDataService;
        private readonly WindowsService _windowsService;
        private int _profit;
        private List<Actor> _playerUnits;

        public GameLoopState(GameStateMachine gameStateMachine,
                             GridDataService gridDataService, 
                             GameplayDataService gameplayService,
                             WaveBuilder waveBuilder,
                             BuffService buffService,
                             UpgradeDataService upgradeDataService,
                             WindowsService windowsService)
        {
            _gameStateMachine = gameStateMachine;
            _gridDataService = gridDataService;
            _gameplayService = gameplayService;
            _waveBuilder = waveBuilder;
            _buffService = buffService;
            _upgradeDataService = upgradeDataService;
            _windowsService = windowsService;
        }

        public void Enter()
        {
            _playerUnits = _gridDataService.PlayerUnits;
            if (_gridDataService.PlayerUnits.Count == 0)
            {
                Lose();
                return;
            }
            
            _profit = _gameplayService.Crowns;
            _gridDataService.Save();
            _buffService.ApplyBuffs(_playerUnits, _waveBuilder.EnemyUnits);
            _upgradeDataService.IncrementStats(_playerUnits);
            
            foreach (Actor actor in _playerUnits)
            {
                actor.Died += OnAllyDied;
                actor.Unleash();
            }

            foreach (Actor actor in _waveBuilder.EnemyUnits)
            {
                actor.Died += OnEnemyDied;
                actor.Unleash();
            }
            
            _windowsService.Show<GameCanvas, GameCanvasData>(new GameCanvasData(false));
        }

        public void Exit()
        {
            _profit = _gameplayService.Crowns - _profit;
            foreach (Actor actor in _waveBuilder.EnemyUnits)
            {
                actor.Died -= OnEnemyDied;
            }

            foreach (Actor actor in _playerUnits)
            {
                actor.Died -= OnAllyDied;
            }
        }

        private void OnAllyDied()
        {
            if (_gridDataService.PlayerUnits.All(a => a.IsDead))
            {
                Lose();
            }
        }

        private void Lose() => _gameStateMachine.Enter<ResultScreenState, ResultScreenData>(new ResultScreenData(false, false ));

        private void OnEnemyDied()
        {
            if (_waveBuilder.EnemyUnits.Count <=0 || _waveBuilder.EnemyUnits.All(a => a.IsDead))
            {
                _gameStateMachine.Enter<ResultScreenState, ResultScreenData>(new ResultScreenData(true, false));
            }
        }
    }
}