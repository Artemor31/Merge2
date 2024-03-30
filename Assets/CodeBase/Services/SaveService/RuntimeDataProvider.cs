using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Units;
using CodeBase.LevelData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Services.SaveService
{
    public class RuntimeDataProvider : IService
    {
        public int Wave => _progress.Wave;
        public Vector2Int GridSize => _progress.GridSize;
        public GridRuntimeData this[Vector2Int index] => GridData[index.x, index.y];
        
        public GridRuntimeData[,] GridData { get; }
        public List<Actor> EnemyUnits { get; }

        private readonly PlayerProgress _progress;

        public RuntimeDataProvider()
        {
            int wave = PlayerPrefs.GetInt("level", 0);
            _progress = new PlayerProgress
            {
                Wave = wave,
                GridSize = new Vector2Int(3, 5)
            };

            GridData = new GridRuntimeData[_progress.GridSize.x, _progress.GridSize.y];

            DoForeach((i, j) => GridData[i, j] = new GridRuntimeData());
            EnemyUnits = new List<Actor>();
        }


        public Platform GetFreePlatform()
        {
            for (int i = 0; i < GridData.GetLength(0); i++)
            {
                for (int j = 0; j < GridData.GetLength(1); j++)
                {
                    if (GridData[i, j].Busy) continue;
                    return GridData[i, j].Platform;
                }
            }

            return null;
        }

        public void AddActor(Actor actor, Platform platform)
        {
            for (int i = 0; i < GridData.GetLength(0); i++)
            {
                for (int j = 0; j < GridData.GetLength(1); j++)
                {
                    if (GridData[i, j].Platform != platform) continue;
                    GridData[i, j].Actor = actor;
                    return;
                }
            }
        }

        public void AddActor(Actor actor)
        {
            for (int i = 0; i < GridData.GetLength(0); i++)
            {
                for (int j = 0; j < GridData.GetLength(1); j++)
                {
                    if (GridData[i, j].Busy) continue;
                    GridData[i, j].Actor = actor;
                    return;
                }
            }
        }

        public void SetupPlatforms(Platform[,] platforms) =>
            DoForeach((i, j) => GridData[i, j].Platform = platforms[i, j].Init(i, j));

        public IReadOnlyList<Actor> GetPlayerUnits()
        {
            List<Actor> units = new();
            DoForeach((i, j) =>
            {
                if (GridData[i, j].Busy)
                    units.Add(GridData[i, j].Actor);
            });

            return units;
        }

        private void DoForeach(Action<int, int> action)
        {
            for (int i = 0; i < GridData.GetLength(0); i++)
                for (int j = 0; j < GridData.GetLength(1); j++)
                    action.Invoke(i, j);
        }

        public void AddEnemy(Actor actor) => EnemyUnits.Add(actor);

        public void RemoveEnemies()
        {
            EnemyUnits.ForEach(Object.Destroy);
            EnemyUnits.Clear();
        }
    }
}