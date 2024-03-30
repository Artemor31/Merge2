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
        public IReadOnlyList<Actor> PlayerUnits => GetPlayerUnits();
        public int Wave => _progress.Wave;
        public Vector2Int GridSize => _progress.GridSize;

        public GridRuntimeData[,] GridData => _gridData;

        public GridRuntimeData this[Vector2Int index]
        {
            get => _gridData[index.x, index.y];
            set => _gridData[index.x, index.y] = value;
        }

        private readonly PlayerProgress _progress;
        private readonly GridRuntimeData[,] _gridData;
        private readonly List<Actor> _enemyUnits;

        public RuntimeDataProvider()
        {
            int wave = PlayerPrefs.GetInt("level", 0);
            _progress = new PlayerProgress
            {
                Wave = wave,
                GridSize = new Vector2Int(3, 5)
            };

            _gridData = new GridRuntimeData[_progress.GridSize.x, _progress.GridSize.y];

            DoForeach((i, j) => _gridData[i, j] = new GridRuntimeData());
            _enemyUnits = new List<Actor>();
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

        private IReadOnlyList<Actor> GetPlayerUnits()
        {
            List<Actor> units = new();
            DoForeach((i, j) =>
            {
                if (GridData[i, j].Busy == false)
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

        public void AddEnemy(Actor actor) => _enemyUnits.Add(actor);

        public void RemoveEnemies()
        {
            _enemyUnits.ForEach(Object.Destroy);
            _enemyUnits.Clear();
        }
    }
}