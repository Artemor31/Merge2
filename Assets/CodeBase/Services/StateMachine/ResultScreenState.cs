using CodeBase.Services.SaveService;
using CodeBase.UI;

namespace CodeBase.Services.StateMachine
{
    public class ResultScreenState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly WindowsService _windowsService;
        private readonly RuntimeDataProvider _dataProvider;
        private readonly GameObserver _gameObserver;

        public ResultScreenState(GameStateMachine stateMachine, 
                                 WindowsService windowsService,
                                 RuntimeDataProvider dataProvider,
                                 GameObserver gameObserver)
        {
            _stateMachine = stateMachine;
            _windowsService = windowsService;
            _dataProvider = dataProvider;
            _gameObserver = gameObserver;
        }
        
        public void Enter()
        {
            if (_gameObserver.IsWin)
            {
                _windowsService.Show<WinResultPresenter>();
            }
            else
            {
                //
            }

        }
    }
}