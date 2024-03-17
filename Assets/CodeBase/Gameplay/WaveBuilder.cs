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
    public class WaveBuilder : IService
    {
        private readonly GameFactory _factory;
        private readonly WavesDatabase _wavesDatabase;

        public WaveBuilder(GameFactory factory, DatabaseProvider provider)
        {
            _factory = factory;
            _wavesDatabase = provider.GetDatabase<WavesDatabase>();
        }

        public void BuildEnemyWave(LevelStaticData staticData, GameplayModel model, int wave)
        {
            CleanUp(model);

            WavesDatabase.WaveData waveData = CurrentWaveData(wave);

            foreach (WavesDatabase.EnemyAmount data in waveData.Enemies)
            {
                for (int i = 0; i < data.Amount; i++)
                {
                    var enemy = _factory.CreateUnit(data.Unit);
                    enemy.transform.position = staticData.EnemyPositions.Random();
                    model.EnemyUnits.Add(enemy);
                }
            }
        }

        public void BuildPlayerWave(LevelStaticData staticData, GameplayModel model)
        {
            foreach (Actor unit in model.PlayerUnits)
            {
                // сохранять id юнита и его позицию?
            }
        }

        private WavesDatabase.WaveData CurrentWaveData(int wave) =>
            _wavesDatabase.WavesData.First(d => d.Wave == wave);

        private void CleanUp(GameplayModel model)
        {
            model.EnemyUnits.ForEach(Object.Destroy);
            model.EnemyUnits.Clear();
        }
    }
}