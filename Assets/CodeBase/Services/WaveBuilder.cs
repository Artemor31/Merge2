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
            WaveData waveData = CurrentWaveData();
            List<Vector3> positions = GetPositions();

            foreach (var data in waveData.Enemies)
            {
                for (int i = 0; i < data.Amount; i++)
                {
                    Actor enemy = _factory.CreateActor(data.ActorData, positions.Random());
                    _enemyUnits.Add(enemy);
                }
            }
        }

        private WaveData CurrentWaveData()
        {
            return _wavesDatabase.WavesData[_playerProgress.Wave];
        }

        private List<Vector3> GetPositions()
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