using System;
using System.Collections.Generic;
using CodeBase.Gameplay;
using CodeBase.LevelData;
using CodeBase.Services.SaveService;

namespace CodeBase.Services.StateMachine
{
    public class GameStateMachine : IService
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        public GameStateMachine(SceneLoader sceneLoader,
                                WindowsService windowsService,
                                WaveBuilder waveBuilder,
                                RuntimeDataRepository runtimeDataRepository,
                                GameFactory gameFactory,
                                GridService gridService,
                                RuntimeDataRepository dataRepository, 
                                GameObserver gameObserver)
        {
            _states = new Dictionary<Type, IState>
            {
                {typeof(BootstrapState), new BootstrapState(this, sceneLoader)},
                {typeof(MenuState), new MenuState(this, windowsService)},
                {typeof(LoadLevelState), new LoadLevelState(this, sceneLoader, waveBuilder, runtimeDataRepository, gameFactory, gridService)},
                {typeof(GameLoopState), new GameLoopState(this, windowsService, gameObserver)},
                {typeof(ResultScreenState), new ResultScreenState(this, windowsService, dataRepository, gameObserver)},
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