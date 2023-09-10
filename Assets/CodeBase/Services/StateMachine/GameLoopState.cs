using CodeBase.Gameplay;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using CodeBase.Models;
using CodeBase.UI.GameplayWindow;

namespace CodeBase.Services.StateMachine
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly WindowsService _windowsService;
        private BattleConductor _conductor;

        public GameLoopState(GameStateMachine gameStateMachine, WindowsService windowsService)
        {
            _gameStateMachine = gameStateMachine;
            _windowsService = windowsService;
        }

        public void Enter()
        {
            var model = ModelsContainer.Resolve<GameplayModel>();
            var staticData = ModelsContainer.Resolve<LevelStaticData>();

            var updatable = ServiceLocator.Resolve<IUpdateable>();
            var waveBuilder = ServiceLocator.Resolve<WaveBuilder>();

            _conductor = new BattleConductor(model, staticData, updatable, waveBuilder);

            // TODO
            _windowsService.Show<GameplayWindow>();
        }
    }
}