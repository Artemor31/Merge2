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
        private readonly Dictionary<int, List<ActorConfig>> _variants = new();

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
            EnemyUnits.Clear();
            WaveData waveData = GetWaveData(wave);

            var positions = _levelDatabase.GetPositions().ToList();
            var datas = CreateActorsWave(waveData, positions.Count);

            foreach (ActorData data in datas)
            {
                if (positions.Count == 0) return;

                var position = positions.Random();
                Actor enemyActor = _factory.CreateEnemyActor(data, position);
                positions.Remove(position);
                enemyActor.transform.Rotate(new Vector3(0, 180, 0));
                EnemyUnits.Add(enemyActor);
            }
        }

        private IEnumerable<ActorData> CreateActorsWave(WaveData waveData, int posLeft)
        {
            Dictionary<int, List<ActorConfig>> variants = FillVariants(waveData);

            int limit = waveData.PowerLimit;
            int maxCost = waveData.MaxLevel switch {1 => 1, 2 => 2, 3 => 4, _ => 1};

            while (limit > 0 && posLeft > 0)
            {
                int range = posLeft >= 5 ? Random.Range(1, 11) : 1;

                if (maxCost == 4 && range < 7) // try spawn lvl 3
                {
                    limit -= 4;
                    yield return variants[3].Random().Data;
                }
                else if (maxCost == 2 && range < 9) // try spawn lvl 2
                {
                    limit -= 2;
                    yield return variants[2].Random().Data;
                }
                else // spawn lvl 1
                {
                    limit -= 1;
                    yield return variants[1].Random().Data;
                }
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

        private Dictionary<int, List<ActorConfig>> FillVariants(WaveData waveData)
        {
            _variants.Clear();
            for (int level = 1; level <= waveData.MaxLevel; level++)
            {
                List<ActorConfig> actorConfigs =
                    _unitsDatabase.ConfigsFor(level, waveData.Races, waveData.Masteries).ToList();
                _variants.Add(level, actorConfigs);
            }

            return _variants;
        }
    }
}