using System.Collections.Generic;
using System.Linq;
using CodeBase.Databases;
using CodeBase.Gameplay.Units;
using CodeBase.Infrastructure;
using CodeBase.LevelData;
using CodeBase.Models;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Gameplay
{
    public class WaveBuilder
    {
        private readonly GameplayFactory _factory;
        private readonly WavesDatabase _wavesDatabase;
        private readonly GameplayModel _model;
        private readonly LevelStaticData _levelStaticData;

        public WaveBuilder(GameplayFactory factory,
                           WavesDatabase wavesDatabase,
                           GameplayModel model,
                           LevelStaticData levelStaticData)
        {
            _wavesDatabase = wavesDatabase;
            _factory = factory;
            _model = model;
            _levelStaticData = levelStaticData;
        }

        public void BuildWave(int wave)
        {
            CleanUp();
            
            var waveData = CurrentWaveData(wave);
            
            foreach (var data in waveData.Enemies)
            {
                for (var i = 0; i < data.Amount; i++)
                {
                    var enemy = _factory.CreateUnit(data.Unit);
                    enemy.transform.position = _levelStaticData.EnemyPositions.Random();
                    _model.EnemyUnits.Add(enemy);
                }
            }
        }

        private WaveData CurrentWaveData(int wave) => 
            _wavesDatabase.WavesData.First(d => d.Wave == wave);

        private void CleanUp()
        {
            _model.EnemyUnits ??= new List<Unit>();
            _model.EnemyUnits.ForEach(Object.Destroy);
            _model.EnemyUnits.Clear();
        }
    }
}