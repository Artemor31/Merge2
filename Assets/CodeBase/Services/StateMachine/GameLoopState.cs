using CodeBase.UI.GameplayWindow;
using CodeBase.Gameplay;

namespace CodeBase.Services.StateMachine
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly WindowsService _windowsService;
        private readonly BattleConductor _conductor;
        
        public GameLoopState(GameStateMachine gameStateMachine, 
                             WindowsService windowsService,
                             BattleConductor conductor)
        {
            _gameStateMachine = gameStateMachine;
            _windowsService = windowsService;
            _conductor = conductor;
        }

        public void Enter()
        {
            _conductor.SetState(GameState.Waiting);
            _windowsService.Show<GameplayPresenter>();
        }
    }
}