using CodeBase.UI.GameplayWindow;
using CodeBase.Gameplay;

namespace CodeBase.Services.StateMachine
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly WindowsService _windowsService;
        private readonly WaveBuilder _waveBuilder;
        private readonly ProgressService _progressService;
        private BattleConductor _conductor;
        
        public GameLoopState(GameStateMachine gameStateMachine, 
                             WindowsService windowsService,
                             WaveBuilder waveBuilder,
                             ProgressService progressService)
        {
            _gameStateMachine = gameStateMachine;
            _windowsService = windowsService;
            _waveBuilder = waveBuilder;
            _progressService = progressService;
        }

        public void Enter()
        {
            _conductor = new BattleConductor(_progressService, _waveBuilder);
            _windowsService.Show<GameplayPresenter>();
        }
    }
}