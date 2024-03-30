using System.Collections.Generic;
using System.Linq;
using CodeBase.Databases;
using CodeBase.Gameplay.Units;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using CodeBase.Services;
using CodeBase.Services.SaveService;

namespace CodeBase.Gameplay
{
    public class WaveBuilder : IService
    {
        private readonly GameFactory _factory;
        private readonly WavesDatabase _wavesDatabase;

        public WaveBuilder(GameFactory factory, DatabaseProvider provider)
        {
            _factory = factory;
            _wavesDatabase = provider.GetDatabase<WavesDatabase>();
        }

        public void BuildEnemyWave(EnemySpawner staticData, RuntimeDataProvider service)
        {
            service.RemoveEnemies();

            foreach (WavesDatabase.EnemyAmount data in WaveData(service.Wave).Enemies)
            {
                for (int i = 0; i < data.Amount; i++)
                {
                    var enemy = _factory.CreateUnit(data.Unit);
                    enemy.transform.position = staticData.EnemyPositions.Random();
                    service.AddEnemy(enemy);
                }
            }
        }

        public void BuildPlayerWave(EnemySpawner staticData, IReadOnlyList<Actor> actors)
        {
            foreach (Actor unit in actors)
            {
                // сохранять id юнита и его позицию?
            }
        }

        private WavesDatabase.WaveData WaveData(int wave) =>
            _wavesDatabase.WavesData.First(d => d.Wave == wave);
    }
}