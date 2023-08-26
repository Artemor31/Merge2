using System.Linq;
using CodeBase.Databases;
using CodeBase.Infrastructure;
using CodeBase.Models;
using CodeBase.Services.StateMachine;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class WaveBuilder
    {
        private readonly UnitsFactory _factory;
        private readonly WavesDatabase _wavesDatabase;
        private readonly GameplayModel _model;

        public WaveBuilder(UnitsFactory factory, WavesDatabase wavesDatabase, GameplayModel model)
        {
            _wavesDatabase = wavesDatabase;
            _factory = factory;
            _model = model;
        }

        public void BuildWave(int wave, LevelStaticData staticData)
        {
            CleanUp();
            
            var waveData = CurrentWaveData(wave);
            
            foreach (var data in waveData.Enemies)
            {
                for (var i = 0; i < data.Amount; i++)
                {
                    var enemy = _factory.CreateUnit(data.Unit);
                    enemy.transform.position = staticData.EnemyPositions.Random();
                    _model.EnemyUnits.Add(enemy);
                }
            }
        }

        private WaveData CurrentWaveData(int wave) => 
            _wavesDatabase.WavesData.First(d => d.Wave == wave);

        private void CleanUp()
        {
            _model.EnemyUnits.ForEach(Object.Destroy);
            _model.EnemyUnits.Clear();
        }
    }
}