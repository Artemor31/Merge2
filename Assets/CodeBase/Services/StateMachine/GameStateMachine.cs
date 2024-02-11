using System;
using System.Collections.Generic;
using CodeBase.Gameplay;
using CodeBase.Infrastructure;

namespace CodeBase.Services.StateMachine
{
    public class GameStateMachine : IService
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        public GameStateMachine(SceneLoader sceneLoader, WindowsService windowsService)
        {
            var bootstrapState = new BootstrapState(this, sceneLoader);
            var loadLevelState = new LoadLevelState(this, sceneLoader);
            var menuState = new MenuState(this, windowsService);
            var waveBuilder = ServiceLocator.Resolve<WaveBuilder>();
            var progressService = ServiceLocator.Resolve<ProgressService>();
            var gameLoopState = new GameLoopState(this, windowsService, waveBuilder, progressService);

            _states = new Dictionary<Type, IState>
            {
                {typeof(BootstrapState), bootstrapState},
                {typeof(LoadLevelState), loadLevelState},
                {typeof(MenuState), menuState},
                {typeof(GameLoopState), gameLoopState},
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