using Services.Infrastructure;
using UI.MenuWindow;

namespace Services.StateMachine
{
    public class MenuState : IState
    {
        private readonly WindowsService _windowsService;

        public MenuState(WindowsService windowsService) => _windowsService = windowsService;
        public void Enter() => _windowsService.Show<MenuPresenter>();
    }
}