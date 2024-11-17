using System.Collections.Generic;
using Data;
using Databases;
using Gameplay.Units;
using Infrastructure;
using Services.SaveService;
using UnityEngine;

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
            List<ActorData> actorsWave = CreateActorsWave(waveData);
            List<Vector3> positions = _levelDatabase.GetPositions();

            foreach (ActorData data in actorsWave)
            {
                _enemyUnits.Add(_factory.CreateEnemyActor(data, positions.Random()));
            }
        }

        private List<ActorData> CreateActorsWave(WaveData waveData)
        {
            List<ActorData> datas = new();
            var variants = FillVariants(waveData);
            int powerLimit = waveData.PowerLimit;
            for (int i = powerLimit; i >= 1;)
            {
                // if est' voobwe s takoi siloi
                // i est' ewe za chto kypit' vraga
                // to roll na to, chto kypim etot lvl vraga
                if (variants[i - 1].Count > 0 && powerLimit >= i)
                {
                    if (powerLimit == i)
                    {
                        if (Roll40Percantage())
                        {
                            datas.Add(variants[i - 1].Random().Data);
                            powerLimit -= i;
                        }
                        else
                        {
                            i--;
                        }
                    }
                    else
                    {
                        datas.Add(variants[i - 1].Random().Data);
                        powerLimit -= i;
                    }
                }
                else
                {
                    i--;
                }
            }

            return datas;
        }

        private static bool Roll40Percantage()
        {
            return Random.Range(1, 11) >= 7;
        }

        private List<List<ActorConfig>> FillVariants(WaveData waveData)
        {
            List<List<ActorConfig>> variants = new();
            for (int i = 1; i <= waveData.PowerLimit; i++)
            {
                variants.Add(_unitsDatabase.ConfigsFor(i, waveData.Races, waveData.Masteries));
            }

            return variants;
        }
    }
}