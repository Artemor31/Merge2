using System.Collections.Generic;
using Databases;
using Infrastructure;
using Services;
using Services.SaveService;
using UnityEngine;

namespace Gameplay
{
    public class WaveBuilder : IService
    {
        private readonly GameFactory _factory;
        private readonly PlayerProgressService _playerProgress;
        private readonly GridDataService _gridDataService;
        private readonly WavesDatabase _wavesDatabase;
        private readonly LevelDatabase _levelDatabase;

        public WaveBuilder(GameFactory factory, 
                           DatabaseProvider provider, 
                           PlayerProgressService playerProgress,
                           GridDataService gridDataService)
        {
            _factory = factory;
            _playerProgress = playerProgress;
            _gridDataService = gridDataService;
            _wavesDatabase = provider.GetDatabase<WavesDatabase>();
            _levelDatabase = provider.GetDatabase<LevelDatabase>();
        }

        public void BuildEnemyWave()
        {
            _gridDataService.RemoveEnemies();
            var enemiesPositions = GetPositions();

            foreach (WavesDatabase.EnemyAmount data in _wavesDatabase.WavesData[_playerProgress.Wave].Enemies)
            {
                for (int i = 0; i < data.Amount; i++)
                {
                    var enemy = _factory.CreateActor(data.Race, data.Mastery, data.Level);
                    enemy.transform.position = enemiesPositions.Random();
                    _gridDataService.AddEnemy(enemy);
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
    }
}