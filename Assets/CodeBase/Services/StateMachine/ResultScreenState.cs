using CodeBase.Services.SaveService;
using CodeBase.UI;

namespace CodeBase.Services.StateMachine
{
    public class ResultScreenState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly WindowsService _windowsService;
        private readonly RuntimeDataRepository _dataRepository;
        private readonly GameObserver _gameObserver;

        public ResultScreenState(GameStateMachine stateMachine, 
                                 WindowsService windowsService,
                                 RuntimeDataRepository dataRepository,
                                 GameObserver gameObserver)
        {
            _stateMachine = stateMachine;
            _windowsService = windowsService;
            _dataRepository = dataRepository;
            _gameObserver = gameObserver;
        }
        
        public void Enter()
        {
            _dataRepository.UninitPlatforms();
            if (_gameObserver.IsWin)
            {
                _windowsService.Show<WinResultPresenter>();
            }
            else
            {
                _windowsService.Show<LoseResultPresenter>();
            }

        }
    }
}