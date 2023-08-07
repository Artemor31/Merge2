using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using CodeBase.Models;

namespace CodeBase.Services.StateMachine
{
    public class GameStateMachine : IService
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        public GameStateMachine(SceneLoader sceneLoader, WindowsService windowsService)
        {
            var gameplayModel = ModelsContainer.Resolve<GameplayModel>();
            var databaseProvider = ServiceLocator.Resolve<DatabaseProvider>();
            
            _states = new Dictionary<Type, IState>
            {
                {typeof(BootstrapState), new BootstrapState(this, sceneLoader)},
                {typeof(LoadLevelState), new LoadLevelState(this, sceneLoader)},
                {typeof(MenuState), new MenuState(this, windowsService)},
                {typeof(GameLoopState), new GameLoopState(this, windowsService, gameplayModel, databaseProvider)},
            };
        }

        public void Enter<T>() where T : IState
        {
            if (_currentState is IExitableState state)
                state.Exit();

            var newState = _states[typeof(T)];
            _currentState = newState;
            _currentState.Enter();
        }
    }
}