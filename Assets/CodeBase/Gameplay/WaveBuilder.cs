using System.Collections.Generic;
using System.Linq;
using CodeBase.Databases;
using CodeBase.Gameplay.Units;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using CodeBase.Services;
using CodeBase.Services.SaveService;
using UnityEngine;

namespace CodeBase.Gameplay
{
    public class WaveBuilder : IService
    {
        private readonly GameFactory _factory;
        private readonly WavesDatabase _wavesDatabase;
        private readonly LevelDatabase _levelDatabase;

        public WaveBuilder(GameFactory factory, DatabaseProvider provider)
        {
            _factory = factory;
            _wavesDatabase = provider.GetDatabase<WavesDatabase>();
            _levelDatabase = provider.GetDatabase<LevelDatabase>();
        }

        public void BuildEnemyWave(RuntimeDataRepository dataRepository)
        {
            dataRepository.RemoveEnemies();
            var enemiesPositions = GetPositions();

            foreach (WavesDatabase.EnemyAmount data in WaveData(dataRepository.Wave).Enemies)
            {
                for (int i = 0; i < data.Amount; i++)
                {
                    var enemy = _factory.CreateActor(data.Unit);
                    enemy.transform.position = enemiesPositions.Random();
                    dataRepository.AddEnemy(enemy);
                }
            }
        }

        private IReadOnlyList<Vector3> GetPositions()
        {
            var positions = new List<Vector3>();
            Vector2 size = _levelDatabase.SpawnerSize;
            for (int i = 0; i < size.x; i++)
            {
                Vector3 position = _levelDatabase.SpawnerPosition;
                float delta = _levelDatabase.SpawnerDelta;
                float currentX = position.x + delta * i;

                for (int j = 0; j < size.y; j++)
                {
                    float currentZ = position.z + delta * j;
                    positions.Add(new Vector3(currentX, 0, currentZ));
                }
            }

            return positions;
        }

        public void BuildPlayerWave(IReadOnlyList<Actor> actors)
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