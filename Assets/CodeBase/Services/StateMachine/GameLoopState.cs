using CodeBase.Models;
using CodeBase.UI.GameplayWindow;

namespace CodeBase.Services.StateMachine
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly WindowsService _windowsService;
        private readonly GameplayModel _model;
        private readonly DatabaseProvider _databaseProvider;
        private BattleProcessor _battleProcessor;

        public GameLoopState(GameStateMachine gameStateMachine,
                             WindowsService windowsService,
                             GameplayModel model,
                             DatabaseProvider databaseProvider)
        {
            _gameStateMachine = gameStateMachine;
            _windowsService = windowsService;
            _model = model;
            _databaseProvider = databaseProvider;
        }

        public void Enter()
        {
            _windowsService.Show<GameplayWindow>();
            _battleProcessor = new BattleProcessor(_model, _databaseProvider);
        }
    }
}