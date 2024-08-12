using UI;
using UI.GameplayWindow;

namespace Services.StateMachine
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly WindowsService _windowsService;
        private readonly GameObserver _gameObserver;

        public GameLoopState(GameStateMachine gameStateMachine,
                             WindowsService windowsService,
                             GameObserver gameObserver)
        {
            _gameStateMachine = gameStateMachine;
            _windowsService = windowsService;
            _gameObserver = gameObserver;
        }

        public void Enter()
        {
            _windowsService.Show<GameplayPresenter>();
            _gameObserver.OnGameplayEnded += MoveNext;
        }

        private void MoveNext(bool isWin)
        {
            _gameObserver.OnGameplayEnded -= MoveNext;
            _gameStateMachine.Enter<ResultScreenState>();
        }
    }
}