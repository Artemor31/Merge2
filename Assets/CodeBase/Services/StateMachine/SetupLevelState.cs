using UI;
using UI.GameplayWindow;
using UI.MenuWindow;

namespace Services.StateMachine
{
    public class SetupLevelState : IState
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
    }
}