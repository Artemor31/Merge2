using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Databases.Data;
using Gameplay.Units;
using Infrastructure;
using Services.Infrastructure;
using Services.Resources;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Services
{
    public class WaveBuilder : IService
    {
        public List<Actor> EnemyUnits { get; }

        private readonly GameplayDataService _gameplayData;
        private readonly TutorialService _tutorialService;
        private readonly WavesDatabase _wavesDatabase;
        private readonly LevelDatabase _levelDatabase;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly GameFactory _factory;

        public WaveBuilder(GameFactory factory, DatabaseProvider provider, GameplayDataService gameplayData, TutorialService tutorialService)
        {
            _wavesDatabase = provider.GetDatabase<WavesDatabase>();
            _levelDatabase = provider.GetDatabase<LevelDatabase>();
            _unitsDatabase = provider.GetDatabase<UnitsDatabase>();
            _gameplayData = gameplayData;
            _tutorialService = tutorialService;
            _factory = factory;
            EnemyUnits = new List<Actor>();
        }

        public void BuildEnemyWave()
        {
            EnemyUnits.ForEach(Object.Destroy);
            EnemyUnits.Clear();
            SpawnUnits();
        }

        private void SpawnUnits()
        {
            WaveData waveData = GetWaveData();

            var positions = _levelDatabase.GetPositions().ToList();

            foreach (ActorData data in CreateActorsWave(waveData))
            {
                var position = positions.Random();
                Actor enemyActor = _factory.CreateEnemyActor(data, position);
                positions.Remove(position);
                enemyActor.transform.Rotate(new Vector3(0,180,0));
                EnemyUnits.Add(enemyActor);
            }
        }

        private WaveData GetWaveData()
        {
            WaveData waveData;
            int count = _wavesDatabase.WavesData.Count;
            int wave = _gameplayData.Wave;

            // TODO
            if (!_tutorialService.EndedTutor)
            {
                return _wavesDatabase.TutorData[wave];
            }

            if (wave < count)
            {
                waveData = _wavesDatabase.WavesData[wave];
            }
            else
            {
                int levelsAhead = wave - count + 1;
                WaveData data = _wavesDatabase.WavesData[^1];
                int tenPercents = Mathf.RoundToInt(data.PowerLimit * 0.1f + 0.5f);
                int powerLimit = data.PowerLimit + tenPercents;
                for (int i = 1; i < levelsAhead; i++)
                {
                    powerLimit += Mathf.RoundToInt(powerLimit * 0.1f + 0.5f);
                }

                waveData = new WaveData
                {
                    MaxLevel = data.MaxLevel,
                    Masteries = data.Masteries,
                    Races = data.Races,
                    PowerLimit = powerLimit
                };
            }

            return waveData;
        }

        private IEnumerable<ActorData> CreateActorsWave(WaveData waveData)
        {
            Dictionary<int, List<ActorConfig>> variants = FillVariants(waveData);
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
                List<ActorConfig> actorConfigs = _unitsDatabase.ConfigsFor(level, waveData.Races, waveData.Masteries).ToList();
                variants.Add(level, actorConfigs);
            }

            return variants;
        }

        private static bool Roll70Percantage() => Random.Range(1, 11) <= 7;
    }
}