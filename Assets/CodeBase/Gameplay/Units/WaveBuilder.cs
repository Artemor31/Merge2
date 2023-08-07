using System.Linq;
using CodeBase.Databases;
using CodeBase.Infrastructure;
using CodeBase.Models;
using UnityEngine;

namespace CodeBase.Gameplay.Units
{
    public class WaveBuilder
    {
        private readonly WavesDatabase _wavesDatabase;
        private readonly GameplayModel _model;

        public WaveBuilder(WavesDatabase wavesDatabase, GameplayModel model)
        {
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
                    var enemy = Object.Instantiate(data.Unit, position, Quaternion.identity);
                    enemy.SetDestination(staticData.PlayerBase);

                    _model.EnemyUnits.Add(enemy);
                }
            }
        }
    }
}