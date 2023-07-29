using CodeBase.Databases;
using CodeBase.Modules.Gameplay;

namespace CodeBase.Services
{
    public class EnemiesController
    {
        private readonly WaveBuilder _waveBuilder;
        private readonly GameplayModel _gameplayModel;
        private readonly LevelStaticData _levelStaticData;
        private int _currentWave;

        public EnemiesController(WaveBuilder waveBuilder, 
                                 GameplayModel gameplayModel,
                                 LevelStaticData levelStaticData)
        {
            _waveBuilder = waveBuilder;
            _gameplayModel = gameplayModel;
            _levelStaticData = levelStaticData;
        }

        public void PrepareNextWave()
        {
            _gameplayModel.EnemyUnits = new();
            _waveBuilder.BuildWave(_currentWave, _levelStaticData);
        }
    }
}