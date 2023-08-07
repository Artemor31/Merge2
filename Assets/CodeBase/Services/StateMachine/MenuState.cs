using CodeBase.UI.MenuWIndow;

namespace CodeBase.Services.StateMachine
{
    public class MenuState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly WindowsService _windowsService;

        public MenuState(GameStateMachine gameStateMachine, WindowsService windowsService)
        {
            _gameStateMachine = gameStateMachine;
            _windowsService = windowsService;
        }

        public void Enter()
        {
            _windowsService.Show<MenuWindow>();
        }
    }
}