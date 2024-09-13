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
        public IReadOnlyList<Actor> EnemyUnits => _enemyUnits;
        
        private readonly PlayerProgressService _playerProgress;
        private readonly WavesDatabase _wavesDatabase;
        private readonly LevelDatabase _levelDatabase;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly GameFactory _factory;
        private readonly List<Actor> _enemyUnits;

        public WaveBuilder(GameFactory factory,
                           DatabaseProvider provider,
                           PlayerProgressService playerProgress)
        {
            _wavesDatabase = provider.GetDatabase<WavesDatabase>();
            _levelDatabase = provider.GetDatabase<LevelDatabase>();
            _unitsDatabase = provider.GetDatabase<UnitsDatabase>();
            _playerProgress = playerProgress;
            _factory = factory;
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
                    _enemyUnits.Add(_factory.CreateActor(data.ActorData, positions.Random()));
                }
            }
        }

        private WaveData CurrentWaveData()
        {
            // hardcode power levels in configs
            int powerLevel = 5;
            Race[] races = {Race.Human};
            Mastery[] masteries = {Mastery.Warrior, Mastery.Ranger};

            // определить пул юнитов доступных
            // определить лимиты по силе
            // выбирать рандомных юнитов

            List<List<ActorConfig>> _variants = new();

            for (int i = powerLevel; i >= 1; i--)
            {
                _variants.Add(_unitsDatabase.ConfigsFor(powerLevel, races, masteries));
            }
            
            while (powerLevel > 0)
            {
                
            }
            
            // спавнить их


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