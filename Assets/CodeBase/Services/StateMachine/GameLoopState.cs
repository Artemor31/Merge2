using CodeBase.Databases;
using CodeBase.Gameplay;
using CodeBase.Gameplay.Units;
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
        private readonly GameplayModel _model;
        private readonly DatabaseProvider _databaseProvider;
        private BattleConductor _conductor;
        private GameplayFactory _factory;
        private WaveBuilder _waveBuilder;

        public GameLoopState(GameStateMachine gameStateMachine,
                             WindowsService windowsService,
                             GameplayModel model,
                             DatabaseProvider databaseProvider)
        {
            _model = model;
            _windowsService = windowsService;
            _gameStateMachine = gameStateMachine;
            _databaseProvider = databaseProvider;
        }

        public void Enter()
        {
            var updatable = ServiceLocator.Resolve<IUpdateable>();
            var assetsProvider = ServiceLocator.Resolve<AssetsProvider>();
            var database = _databaseProvider.GetDatabase<UnitsDatabase>();
            var wavesDatabase = _databaseProvider.GetDatabase<WavesDatabase>();
            var levelStaticData = ModelsContainer.Resolve<LevelStaticData>();

            _model.PlayerUnits = new();


            _factory = new GameplayFactory(database, assetsProvider);
            _waveBuilder = new WaveBuilder(_factory, wavesDatabase, _model, levelStaticData);
            _conductor = new BattleConductor(_model, updatable, _waveBuilder);

            _waveBuilder.BuildWave(0);

            _windowsService.Show<GameplayWindow>();
        }
    }
}