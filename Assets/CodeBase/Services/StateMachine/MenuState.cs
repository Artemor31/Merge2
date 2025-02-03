using Services.Infrastructure;
using UI;

namespace Services.StateMachine
{
    public class MenuState : IState
    {
        private readonly TutorialService _tutorialService;
        private readonly WindowsService _windowsService;

        public MenuState(TutorialService tutorialService, WindowsService windowsService)
        {
            _tutorialService = tutorialService;
            _windowsService = windowsService;
        }

        public void Enter()
        {
            _windowsService.Show<MenuPresenter>();

            if (!_tutorialService.SeenTutor)
            {
                _tutorialService.StartTutor();
                _windowsService.Show<TutorPresenter>();
            }
        }
    }
}