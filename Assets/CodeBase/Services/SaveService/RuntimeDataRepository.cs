using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Units;
using CodeBase.LevelData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Services.SaveService
{
    public class RuntimeDataRepository : IService
    {
        public event Action<int> OnMoneyChanged;
        public event Action<GridRuntimeData> OnPlatformClicked;
        public event Action<GridRuntimeData> OnPlatformReleased;
        public event Action<GridRuntimeData> OnPlatformHovered;

        public int Wave => _progress.Wave;
        public int Money => _progress.Money;
        public Vector2Int GridSize => _progress.GridSize;
        public List<Actor> EnemyUnits { get; }

        private readonly GridRuntimeData[,] _gridData;
        private readonly PlayerProgress _progress;
        private Vector2Int? _selected;

        public RuntimeDataRepository()
        {
            _progress = new PlayerProgress();
            EnemyUnits = new List<Actor>();
            _gridData = new GridRuntimeData[_progress.GridSize.x, _progress.GridSize.y];
        }

        public void InitPlatforms(Platform[,] platforms)
        {
            DoForeach((i, j) =>
            {
                _gridData[i, j] = new GridRuntimeData();
                _gridData[i, j].Platform = platforms[i, j];
                _gridData[i, j].Platform.Init(i, j);
                _gridData[i, j].Platform.OnClicked += PlatformOnOnClicked;
                _gridData[i, j].Platform.OnReleased += PlatformOnOnReleased;
                _gridData[i, j].Platform.OnHovered += PlatformOnOnHovered;
            });
        }

        public void UninitPlatforms()
        {
            DoForeach((i, j) =>
            {
                _gridData[i, j].Platform.OnClicked -= PlatformOnOnClicked;
                _gridData[i, j].Platform.OnReleased -= PlatformOnOnReleased;
                _gridData[i, j].Platform.OnHovered -= PlatformOnOnHovered;
                Object.Destroy(_gridData[i,j].Platform);
            });
        }

        public void AddEnemy(Actor actor) => EnemyUnits.Add(actor);
        public GridRuntimeData GetDataAt(Vector2Int selected) => _gridData[selected.x, selected.y];

        public void RemoveEnemies()
        {
            EnemyUnits.ForEach(Object.Destroy);
            EnemyUnits.Clear();
        }

        public Platform GetFreePlatform()
        {
            for (int i = 0; i < _gridData.GetLength(0); i++)
            {
                for (int j = 0; j < _gridData.GetLength(1); j++)
                {
                    if (_gridData[i, j].Busy) continue;
                    return _gridData[i, j].Platform;
                }
            }

            return null;
        }

        public void AddPlayerUnit(Actor actor, Platform platform)
        {
            for (int i = 0; i < _gridData.GetLength(0); i++)
            {
                for (int j = 0; j < _gridData.GetLength(1); j++)
                {
                    if (_gridData[i, j].Platform != platform) continue;
                    _gridData[i, j].Actor = actor;
                    return;
                }
            }
        }

        public List<Actor> GetPlayerUnits()
        {
            List<Actor> units = new();
            DoForeach((i, j) =>
            {
                if (_gridData[i, j].Busy)
                    units.Add(_gridData[i, j].Actor);
            });

            return units;
        }

        public bool TryBuy(int cost)
        {
            if (!(_progress.Money >= cost)) return false;

            _progress.Money -= cost;
            OnMoneyChanged?.Invoke(_progress.Money);
            return true;
        }

        private void DoForeach(Action<int, int> action)
        {
            for (int i = 0; i < _gridData.GetLength(0); i++)
                for (int j = 0; j < _gridData.GetLength(1); j++)
                    action.Invoke(i, j);
        }
        
        private void PlatformOnOnHovered(Vector2Int vect) => OnPlatformHovered?.Invoke(_gridData[vect.x, vect.y]);
        private void PlatformOnOnReleased(Vector2Int vect) => OnPlatformReleased?.Invoke(_gridData[vect.x, vect.y]);
        private void PlatformOnOnClicked(Vector2Int vect) => OnPlatformClicked?.Invoke(_gridData[vect.x, vect.y]);
    }
}