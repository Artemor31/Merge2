using System.Linq;
using CodeBase.Databases;
using CodeBase.Infrastructure;
using CodeBase.Models;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class WaveBuilder
    {
        private readonly UnitsDatabase _unitsDatabase;
        private readonly WavesDatabase _wavesDatabase;
        private readonly GameplayModel _model;

        public WaveBuilder(UnitsDatabase unitsDatabase, WavesDatabase wavesDatabase, GameplayModel model)
        {
            _unitsDatabase = unitsDatabase;
            _wavesDatabase = wavesDatabase;
            _model = model;
        }

        public void BuildWave(int wave, LevelStaticData staticData)
        {
            var waveData = _wavesDatabase.WavesData.First(d => d.Wave == wave);
    
            foreach (var data in waveData.Enemies)
            {
                for (var i = 0; i < data.Amount; i++)
                {
                    var position = staticData.EnemyPositions.Random();
                    var prefab = _unitsDatabase.Units.First(u => u.Id == data.Unit).Prefab;
                    var enemy = Object.Instantiate(prefab, position, Quaternion.identity);

                    _model.EnemyUnits.Add(enemy);
                }
            }
        }
    }
}