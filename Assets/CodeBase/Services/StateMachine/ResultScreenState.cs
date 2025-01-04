using Services.GridService;
using Services.Infrastructure;
using UI;
using UI.ResultWindow;

namespace Services.StateMachine
{
    public class ResultScreenState : IState<bool>
    {
        private readonly WindowsService _windowsService;
        private readonly GridDataService _gridData;
        private readonly GameplayDataService _gameplayService;

        public ResultScreenState(WindowsService windowsService, GridDataService gridData, GameplayDataService gameplayService)
        {
            _windowsService = windowsService;
            _gridData = gridData;
            _gameplayService = gameplayService;
        }

        public void Enter(bool isWin)
        {
            _gridData.Save();
            
            if (isWin)
            {
                _windowsService.Show<WinResultPresenter>();
                _gameplayService.CompleteLevel();
            }
            else
            {
                _windowsService.Show<LoseResultPresenter>();
            }
        }

        public void Enter()
        {
            
        }
    }
}