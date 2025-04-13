using Infrastructure;
using Services.DataServices;
using Services.Infrastructure;
using UI.MenuWindow;

namespace Services.StateMachine
{
    public class MenuState : IExitableState
    {
        private readonly WindowsService _windowsService;
        private readonly PersistantDataService _persistantDataService;
        private readonly TutorialService _tutorialService;
        private readonly GameStateMachine _gameStateMachine;

        public MenuState(WindowsService windowsService, 
                         PersistantDataService persistantDataService,
                         TutorialService tutorialService,
                         GameStateMachine gameStateMachine)
        {
            _windowsService = windowsService;
            _persistantDataService = persistantDataService;
            _tutorialService = tutorialService;
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            if (_persistantDataService.MaxWave <= 1 ||
                !_tutorialService.SeenTutor)
            {
                _gameStateMachine.Enter<LoadLevelState>();
            }
            else
            {
                _windowsService.Show<MenuPresenter>();
            }
        }

        public void Exit() => _windowsService.Close<MenuPresenter>();
    }
}