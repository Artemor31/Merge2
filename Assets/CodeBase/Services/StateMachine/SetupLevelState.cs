﻿using Services.Infrastructure;
using UI.GameplayWindow;

namespace Services.StateMachine
{
    public class SetupLevelState : IState
    {
        private readonly WindowsService _windowsService;

        public SetupLevelState(WindowsService windowsService) => _windowsService = windowsService;

        public void Enter()
        {
            _windowsService.Show<GameplayPresenter>();
            _windowsService.Show<GameCanvas, GameCanvasData>(new GameCanvasData(true));
        }
    }
}