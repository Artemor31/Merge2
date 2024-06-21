using CodeBase.UI.GameplayWindow;

namespace CodeBase.Services.StateMachine
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly WindowsService _windowsService;
        
        public GameLoopState(GameStateMachine gameStateMachine, WindowsService windowsService)
        {
            _gameStateMachine = gameStateMachine;
            _windowsService = windowsService;
        }

        public void Enter()
        {
            _windowsService.Show<GameplayPresenter>();
        }
    }
}