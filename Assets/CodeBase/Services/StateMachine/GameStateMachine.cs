using UI;
using System;
using Services.SaveService;
using System.Collections.Generic;
using UnityEngine;

namespace Services.StateMachine
{
    public class GameStateMachine : IService
    {
        public event Action<IState> OnStateChanged;
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        public GameStateMachine(SceneLoader sceneLoader,
                                WindowsService windowsService,
                                WaveBuilder waveBuilder,
                                GridDataService gridDataService,
                                GridDataService service,
                                GameplayDataService gameplayData,
                                GridLogicService gridLogicService)
        {
            _states = new Dictionary<Type, IState>
            {
                {typeof(BootstrapState), new BootstrapState(this, sceneLoader)},
                {typeof(MenuState), new MenuState(this, windowsService)},
                {typeof(LoadLevelState), new LoadLevelState(this, sceneLoader, waveBuilder, gridLogicService)},
                {typeof(SetupLevelState), new SetupLevelState(windowsService)},
                {typeof(GameLoopState), new GameLoopState(this, gridDataService, gameplayData, waveBuilder)},
                {typeof(ResultScreenState), new ResultScreenState(windowsService, service, gameplayData)},
            };
        }

        public void Enter<T>() where T : IState
        {
            if (_currentState?.GetType() == typeof(T)) return;

            if (_currentState is IExitableState state)
                state.Exit();
            
            _currentState = _states[typeof(T)];
            _currentState.Enter();
            OnStateChanged?.Invoke(_currentState);
        }
        
        public void Enter<T, TParam>(TParam param) where T : IState
        {
            if (_currentState?.GetType() == typeof(T)) return;
            
            if (_currentState is IExitableState state)
                state.Exit();

            _currentState = _states[typeof(T)];
            ((IState<TParam>)_currentState).Enter(param);          
            OnStateChanged?.Invoke(_currentState);
        }
    }
}