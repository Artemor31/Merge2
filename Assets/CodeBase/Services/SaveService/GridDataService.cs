using System;
using System.Collections.Generic;
using CodeBase.Databases;
using CodeBase.Gameplay.Units;
using CodeBase.LevelData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Services.SaveService
{
    public class GridDataService : IService
    {
        private readonly GameFactory _gameFactory;
        public event Action<GridRuntimeData> OnPlatformClicked;
        public event Action<GridRuntimeData> OnPlatformReleased;
        public event Action<GridRuntimeData> OnPlatformHovered;

        public Vector2Int GridSize = new(3, 5);
        public List<Actor> EnemyUnits { get; }

        private readonly GridRuntimeData[,] _gridData;
        private Vector2Int? _selected;  
        private readonly GridRepository _gridRepo;

        public GridDataService(GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            EnemyUnits = new List<Actor>();
            _gridData = new GridRuntimeData[GridSize.x, GridSize.y];
            _gridRepo = new GridRepository();
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

            GridData restore = _gridRepo.Restore();
            if (restore?.UnitIds != null)
            {
                DoForeach((i, j) =>
                {
                    GridRuntimeData data = _gridData[i, j];
                    UnitData unitData = restore.UnitIds[i, j];
                    if (data == null || unitData.Equals(UnitData.None)) return;
                    
                    data.Actor = _gameFactory.CreateActor(unitData.Race, unitData.Mastery, unitData.Level, data.Platform);
                    AddPlayerUnit(data.Actor, data.Platform);
                });
            }
        }

        public void UninitPlatforms()
        {
            DoForeach((i, j) =>
            {
                _gridData[i, j].Platform.OnClicked -= PlatformOnOnClicked;
                _gridData[i, j].Platform.OnReleased -= PlatformOnOnReleased;
                _gridData[i, j].Platform.OnHovered -= PlatformOnOnHovered;
                Object.Destroy(_gridData[i, j].Platform);
                _gridData[i, j] = null;
            });
        }
        
        public void Save()
        {
            var unitDatas = new UnitData[GridSize.x, GridSize.y];
            DoForeach((i, j) =>
            {
                if (_gridData[i, j].Busy)
                {
                    Actor actor = _gridData[i, j].Actor;
                    unitDatas[i, j] =  new UnitData(actor.Race, actor.Mastery, actor.Level);
                }
                else
                {
                    unitDatas[i, j] = UnitData.None;
                }
            });

            _gridRepo.Save(new GridData(unitDatas));
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