using System.Collections.Generic;
using Databases;
using Gameplay.Units;
using Infrastructure;
using Services.SaveService;
using UnityEngine;

namespace Services
{
    public class WaveBuilder : IService
    {
        private readonly GameFactory _factory;
        private readonly PlayerProgressService _playerProgress;
        private readonly WavesDatabase _wavesDatabase;
        private readonly LevelDatabase _levelDatabase;
        
        public IReadOnlyList<Actor> EnemyUnits => _enemyUnits;
        private readonly List<Actor> _enemyUnits;


        public WaveBuilder(GameFactory factory, 
                           DatabaseProvider provider, 
                           PlayerProgressService playerProgress)
        {
            _factory = factory;
            _playerProgress = playerProgress;
            _wavesDatabase = provider.GetDatabase<WavesDatabase>();
            _levelDatabase = provider.GetDatabase<LevelDatabase>();
            _enemyUnits = new List<Actor>();
        }

        public void BuildEnemyWave()
        {
            _enemyUnits.ForEach(Object.Destroy);
            _enemyUnits.Clear();
            BuildWave();
        }

        private void BuildWave()
        {
            var enemyDatas = _wavesDatabase.WavesData[_playerProgress.Wave].Enemies;
            var positions = GetPositions();

            foreach (var data in enemyDatas)
            {
                for (int i = 0; i < data.Amount; i++)
                {
                    var enemy = _factory.CreateActor(data.Race, data.Mastery, data.Level);
                    enemy.transform.position = positions.Random();
                    _enemyUnits.Add(enemy);
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