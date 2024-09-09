using System;
using System.Collections.Generic;
using Services.SaveService;
using UI;

namespace Services.StateMachine
{
    public class GameStateMachine : IService
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        public GameStateMachine(SceneLoader sceneLoader,
                                WindowsService windowsService,
                                WaveBuilder waveBuilder,
                                GridDataService gridDataService,
                                GameFactory gameFactory,
                                GridViewService gridViewService,
                                GridDataService service,
                                PlayerProgressService playerProgress)
        {
            _states = new Dictionary<Type, IState>
            {
                {typeof(BootstrapState), new BootstrapState(this, sceneLoader)},
                {typeof(MenuState), new MenuState(this, windowsService)},
                {typeof(LoadLevelState), new LoadLevelState(this, sceneLoader, waveBuilder, gridDataService, gameFactory, gridViewService)},
                {typeof(SetupLevelState), new SetupLevelState(windowsService)},
                {typeof(GameLoopState), new GameLoopState(this, gridDataService, playerProgress, waveBuilder)},
                {typeof(ResultScreenState), new ResultScreenState(windowsService, service, playerProgress)},
            };
        }

        public void Enter<T>() where T : IState
        {
            if (_currentState is IExitableState state)
                state.Exit();

            IState newState = _states[typeof(T)];
            _currentState = newState;
            _currentState.Enter();
        }
    }
}