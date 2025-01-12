﻿using System;
using System.Collections.Generic;
using Services.Buffs;
using Services.GridService;
using Services.Infrastructure;
using UI.UpgradeWindow;

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
                                GridDataService service,
                                GameplayDataService gameplayData,
                                GridLogicService gridLogicService,
                                BuffService buffService,
                                UpgradeDataService upgradeDataService, 
                                PersistantDataService persistantDataService)
        {
            _states = new Dictionary<Type, IState>
            {
                {typeof(BootstrapState), new BootstrapState(this, sceneLoader, windowsService)},
                {typeof(MenuState), new MenuState(this, windowsService)},
                {typeof(LoadLevelState), new LoadLevelState(this, sceneLoader, waveBuilder, gridLogicService, windowsService)},
                {typeof(SetupLevelState), new SetupLevelState(windowsService)},
                {typeof(GameLoopState), new GameLoopState(this, gridDataService, gameplayData, waveBuilder, buffService, upgradeDataService, windowsService)},
                {typeof(ResultScreenState), new ResultScreenState(windowsService, service, gameplayData, waveBuilder, persistantDataService)},
            };
        }

        public void Enter<T>() where T : IState
        {
            if (_currentState?.GetType() == typeof(T)) return;

            if (_currentState is IExitableState state)
                state.Exit();
            
            _currentState = _states[typeof(T)];
            _currentState.Enter();
        }
        
        public void Enter<T, TParam>(TParam param) where T : IState
        {
            if (_currentState?.GetType() == typeof(T)) return;
            
            if (_currentState is IExitableState state)
                state.Exit();

            _currentState = _states[typeof(T)];
            ((IState<TParam>)_currentState).Enter(param);          
        }
    }
}