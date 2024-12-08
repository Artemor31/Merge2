using Services.Infrastructure;
using UI;
using UI.GameplayWindow;

namespace Services.StateMachine
{
    public class SetupLevelState : IState, IExitableState
    {
        private readonly WindowsService _windowsService;

        public SetupLevelState(WindowsService windowsService)
        {
            _windowsService = windowsService;
        }
        
        public void Enter()
        {
            _windowsService.Show<GameplayPresenter>();  
            _windowsService.Show<BuffInfoPresenter>();  
        }

        public void Exit()
        {
            _windowsService.Close<BuffInfoPresenter>();  
        }
    }
}