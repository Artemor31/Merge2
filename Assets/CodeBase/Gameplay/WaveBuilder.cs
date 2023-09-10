using System.Linq;
using CodeBase.Databases;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using CodeBase.Models;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Gameplay
{
    public class WaveBuilder : IService
    {
        private readonly GameFactory _factory;
        private readonly GameplayModel _model;
        private readonly WavesDatabase _wavesDatabase;

        public WaveBuilder(GameFactory factory, DatabaseProvider provider)
        {
            _factory = factory;

            _model = ModelsContainer.Resolve<GameplayModel>();
            _wavesDatabase = provider.GetDatabase<WavesDatabase>();
        }

        public void BuildWave(LevelStaticData levelStaticData, int wave)
        {
            CleanUp();

            var waveData = CurrentWaveData(wave);

            foreach (var data in waveData.Enemies)
            {
                for (var i = 0; i < data.Amount; i++)
                {
                    var enemy = _factory.CreateUnit(data.Unit);
                    enemy.transform.position = levelStaticData.EnemyPositions.Random();
                    _model.EnemyUnits.Add(enemy);
                }
            }
        }

        private WaveData CurrentWaveData(int wave) =>
            _wavesDatabase.WavesData.First(d => d.Wave == wave);

        private void CleanUp()
        {
            _model.EnemyUnits.ForEach(Object.Destroy);
            _model.EnemyUnits.Clear();
        }
    }
}