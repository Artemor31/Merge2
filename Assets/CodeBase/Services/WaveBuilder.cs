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

        private readonly TutorialService _tutorialService;
        private readonly WavesDatabase _wavesDatabase;
        private readonly LevelDatabase _levelDatabase;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly GameFactory _factory;

        public WaveBuilder(GameFactory factory, 
                           DatabaseProvider provider,
                           TutorialService tutorialService)
        {
            _wavesDatabase = provider.GetDatabase<WavesDatabase>();
            _levelDatabase = provider.GetDatabase<LevelDatabase>();
            _unitsDatabase = provider.GetDatabase<UnitsDatabase>();
            _tutorialService = tutorialService;
            _factory = factory;
            EnemyUnits = new List<Actor>();
        }

        public void BuildEnemyWave(int wave)
        {
            EnemyUnits.ForEach(Object.Destroy);
            EnemyUnits.Clear();
            SpawnUnits(wave);
        }

        private void SpawnUnits(int wave)
        {
            WaveData waveData = GetWaveData(wave);

            var positions = _levelDatabase.GetPositions().ToList();

            foreach (ActorData data in CreateActorsWave(waveData, positions.Count))
            {
                var position = positions.Random();
                Actor enemyActor = _factory.CreateEnemyActor(data, position);
                positions.Remove(position);
                enemyActor.transform.Rotate(new Vector3(0, 180, 0));
                EnemyUnits.Add(enemyActor);
            }
        }

        private WaveData GetWaveData(int wave)
        {
            WaveData waveData;
            int count = _wavesDatabase.WavesData.Count;

            if (_tutorialService.InTutor)
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

        private IEnumerable<ActorData> CreateActorsWave(WaveData waveData, int posLeft)
        {
            Dictionary<int, List<ActorConfig>> variants = FillVariants(waveData);
            
            int limit = waveData.PowerLimit;
            int maxCost = waveData.MaxLevel switch {1 => 1, 2 => 2, 3 => 4, _ => 1};

            while (limit > 0 && posLeft > 0)
            {
                int range = Random.Range(1, 11);
                int range2 = Random.Range(1, 11);
                
                if (range > 3 || posLeft < 5) // try spawn max level unit
                {
                    limit -= maxCost;
                    yield return variants[maxCost].Random().Data;
                }
                else if (range2 > 3 && maxCost == 4) // if max lvl was == 3 try spawn lvl 2
                {
                    maxCost = 2;
                    limit -= maxCost;
                    yield return variants[maxCost].Random().Data;
                } 
                else // spawn lvl 1 anyway
                {
                    maxCost = 1;
                    limit -= maxCost;
                    yield return variants[maxCost].Random().Data;
                }
            }
        }

        private readonly Dictionary<int, List<ActorConfig>> _variants = new();
        private Dictionary<int, List<ActorConfig>> FillVariants(WaveData waveData)
        {
            _variants.Clear();
            for (int level = 1; level <= waveData.MaxLevel; level++)
            {
                List<ActorConfig> actorConfigs = _unitsDatabase.ConfigsFor(level, waveData.Races, waveData.Masteries).ToList();
                _variants.Add(level, actorConfigs);
            }

            return _variants;
        }
    }
}