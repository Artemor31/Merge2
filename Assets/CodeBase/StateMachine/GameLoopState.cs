using System;
using System.Linq;
using CodeBase.Modules.Gameplay;
using CodeBase.UI;

namespace CodeBase.StateMachine
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly WindowsService _windowsService;

        public GameLoopState(GameStateMachine gameStateMachine, WindowsService windowsService)
        {
            _gameStateMachine = gameStateMachine;
            _windowsService = windowsService;
        }

        public void Enter()
        {
            _windowsService.Show<GameplayWindow>();
        }
    }

    public class GameStateObserver
    {
        private readonly GameplayModel _gameplayModel;

        public GameStateObserver(GameplayModel gameplayModel)
        {
            _gameplayModel = gameplayModel;
            _gameplayModel.StateChanged += GameplayModelOnStateChanged;
        }

        private void GameplayModelOnStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Waiting:
                    SetIdle();
                    break;
                
                case GameState.Processing:
                    
                    break;
                
                case GameState.Shopping:
                    SetIdle();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void SetIdle()
        {
            var allUnits = _gameplayModel.EnemyUnits.Concat(_gameplayModel.PlayerUnits);
            foreach (var unit in allUnits)
            {
                unit.SetIdle();
            }

            return;
        }
    }
}