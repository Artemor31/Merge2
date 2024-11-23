using System;
using System.Collections.Generic;
using Data;
using Databases;
using Gameplay.Units;
using Infrastructure;
using Services.SaveService;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Services
{
    public class WaveBuilder : IService
    {
        public ICollection<Actor> EnemyUnits => _enemyUnits;

        private readonly PlayerDataService _playerData;
        private readonly WavesDatabase _wavesDatabase;
        private readonly LevelDatabase _levelDatabase;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly GameFactory _factory;
        private readonly List<Actor> _enemyUnits;

        public WaveBuilder(GameFactory factory,
                           DatabaseProvider provider,
                           PlayerDataService playerData)
        {
            _wavesDatabase = provider.GetDatabase<WavesDatabase>();
            _levelDatabase = provider.GetDatabase<LevelDatabase>();
            _unitsDatabase = provider.GetDatabase<UnitsDatabase>();
            _playerData = playerData;
            _factory = factory;
            _enemyUnits = new List<Actor>();
        }

        public void BuildEnemyWave()
        {
            _enemyUnits.ForEach(Object.Destroy);
            _enemyUnits.Clear();
            SpawnUnits();
        }

        private void SpawnUnits()
        {
            WaveData waveData = _wavesDatabase.WavesData[_playerData.Wave];
            List<Vector3> positions = _levelDatabase.GetPositions();

            foreach (ActorData data in CreateActorsWave(waveData))
            {
                _enemyUnits.Add(_factory.CreateEnemyActor(data, positions.Random()));
            }
        }

        private IEnumerable<ActorData> CreateActorsWave(WaveData waveData)
        {
            var variants = FillVariants(waveData);
            int limit = waveData.PowerLimit;

            while (limit > 0)
            {
                int currentLevel = Math.Min(waveData.MaxLevel, limit);
                while (currentLevel > 1)
                {
                    if (Roll70Percantage())
                    {
                        limit -= currentLevel;
                        yield return variants[currentLevel].Random().Data;
                        break;
                    }

                    currentLevel--;
                }

                limit--;
                yield return variants[1].Random().Data;
            }
        }

        private Dictionary<int, List<ActorConfig>> FillVariants(WaveData waveData)
        {
            Dictionary<int, List<ActorConfig>> variants = new();
            for (int level = 1; level <= waveData.MaxLevel; level++)
            {
                variants.Add(level, _unitsDatabase.ConfigsFor(level, waveData.Races, waveData.Masteries));
            }

            return variants;
        }

        private static bool Roll70Percantage() => Random.Range(1, 11) <= 7;
    }
}