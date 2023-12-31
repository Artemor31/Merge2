﻿using System;
using CodeBase.Gameplay;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using CodeBase.Models;
using CodeBase.UI.GameplayWindow;
using Object = UnityEngine.Object;

namespace CodeBase.Services.StateMachine
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly WindowsService _windowsService;
        private BattleConductor _conductor;

        public GameLoopState(GameStateMachine gameStateMachine, WindowsService windowsService)
        {
            _gameStateMachine = gameStateMachine;
            _windowsService = windowsService;
        }

        public void Enter()
        {
            var waveBuilder = ServiceLocator.Resolve<WaveBuilder>();
            var progressService = ServiceLocator.Resolve<ProgressService>();

            _conductor = new BattleConductor(progressService, waveBuilder);
            _windowsService.Show<GameplayWindow>();
        }
    }
}