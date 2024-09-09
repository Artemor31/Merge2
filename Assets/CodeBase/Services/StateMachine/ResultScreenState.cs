using Services.SaveService;
using UI;

namespace Services.StateMachine
{
    public class ResultScreenState : IState
    {
        private readonly WindowsService _windowsService;
        private readonly GridDataService _gridData;
        private readonly PlayerProgressService _playerService;

        public ResultScreenState(WindowsService windowsService,
                                 GridDataService gridData,
                                 PlayerProgressService playerService)
        {
            _windowsService = windowsService;
            _gridData = gridData;
            _playerService = playerService;
        }
        
        public void Enter()
        {
            _gridData.Save();
            _gridData.Dispose();
            bool IsWin = true;
            if (IsWin)
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