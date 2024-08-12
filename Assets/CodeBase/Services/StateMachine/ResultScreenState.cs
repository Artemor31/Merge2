using Services.SaveService;
using UI;

namespace Services.StateMachine
{
    public class ResultScreenState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly WindowsService _windowsService;
        private readonly GridDataService _service;
        private readonly GameObserver _gameObserver;
        private readonly PlayerProgressService _playerService;

        public ResultScreenState(GameStateMachine stateMachine, 
                                 WindowsService windowsService,
                                 GridDataService service,
                                 GameObserver gameObserver,
                                 PlayerProgressService playerService)
        {
            _stateMachine = stateMachine;
            _windowsService = windowsService;
            _service = service;
            _gameObserver = gameObserver;
            _playerService = playerService;
        }
        
        public void Enter()
        {
            _service.Save();
            _service.UninitPlatforms();
            if (_gameObserver.IsWin)
            {
                _windowsService.Show<WinResultPresenter>();
                _playerService.CompleteLevel();
            }
            else
            {
                _windowsService.Show<LoseResultPresenter>();
            }

        }
    }
}