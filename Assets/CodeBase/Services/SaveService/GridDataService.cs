using System;
using System.Collections.Generic;
using Data;
using Databases;
using Gameplay.LevelItems;
using Gameplay.Units;
using UnityEngine;

namespace Services.SaveService
{
    public class GridDataService : IService
    {
        public event Action<GridRuntimeData> OnPlatformClicked;
        public event Action<GridRuntimeData> OnPlatformReleased;
        public event Action<GridRuntimeData> OnPlatformHovered;
        
        public IReadOnlyList<Actor> PlayerUnits => GetPlayerUnits();

        private readonly GridRepository _gridRepo;
        private readonly GameFactory _factory;
        private GridRuntimeData[,] _gridData;
        private Vector2Int _gridSize = new(3, 5);
        private Vector2Int? _selected;

        public GridDataService(GameFactory factory)
        {
            _factory = factory;
            _gridData = new GridRuntimeData[_gridSize.x, _gridSize.y];
            _gridRepo = new GridRepository();
            _gridRepo.Restore();
        }

        public void SpawnPlatforms()
        {
            Platform[,] platforms =  _factory.CreatePlatforms(_gridSize);
            
            DoForeach((i, j) =>
            {
                _gridData[i, j] = new GridRuntimeData();
                _gridData[i, j].Platform = platforms[i, j];
                _gridData[i, j].Platform.Init(i, j);
                _gridData[i, j].Platform.OnClicked += PlatformOnOnClicked;
                _gridData[i, j].Platform.OnReleased += PlatformOnOnReleased;
                _gridData[i, j].Platform.OnHovered += PlatformOnOnHovered;
            });

            Restore();
        }
        
        public void Dispose()
        {
            DoForeach((i, j) =>
            {
                _gridData[i, j].Platform.OnClicked -= PlatformOnOnClicked;
                _gridData[i, j].Platform.OnReleased -= PlatformOnOnReleased;
                _gridData[i, j].Platform.OnHovered -= PlatformOnOnHovered;
                if (_gridData[i, j].Actor != null)
                {
                    _gridData[i,j].Actor.Dispose();
                }
                _gridData = null;
            });
        }
        
        public GridRuntimeData GetDataAt(Vector2Int selected) => _gridData[selected.x, selected.y];
        public bool HasFreePlatform() => FreePlatform() != null;
        public void AddPlayerUnit(ActorConfig config) => AddUnit(_factory.CreateActor(config), FreePlatform());

        private List<Actor> GetPlayerUnits()
        {
            List<Actor> units = new();
            DoForeach((i, j) =>
            {
                if (_gridData[i, j].Busy)
                    units.Add(_gridData[i, j].Actor);
            });

            return units;
        }

        private Platform FreePlatform()
        {
            Platform platform = null;
            DoForeach((i, j) =>
            {
                if (_gridData[i, j].Busy == false)
                    platform = _gridData[i, j].Platform;
            });
            return platform;
        }

        private void AddUnit(Actor actor, Platform platform)
        {
            for (int i = 0; i < _gridData.GetLength(0); i++)
            {
                for (int j = 0; j < _gridData.GetLength(1); j++)
                {
                    if (_gridData[i, j].Platform != platform) continue;
                    _gridData[i, j].Actor = actor;
                    actor.transform.position = platform.transform.position;
                    return;
                }
            }
        }

        public void Save()
        {
            var unitDatas = new ActorData[_gridSize.x, _gridSize.y];
            DoForeach((i, j) =>
            {
                if (_gridData[i, j].Busy)
                {
                    Actor actor = _gridData[i, j].Actor;
                    unitDatas[i, j] = actor.Data;
                }
                else
                {
                    unitDatas[i, j] = ActorData.None;
                }
            });

            _gridRepo.Save(new GridData(unitDatas));
        }

        private void Restore()
        {
            ActorData[,] actors = _gridRepo.Restore()?.UnitIds;
            if (actors == null) return;
            
            DoForeach((i, j) =>
            {
                GridRuntimeData data = _gridData[i, j];
                if (data != null && !actors[i, j].Equals(ActorData.None))
                {
                    data.Actor = _factory.CreateActor(actors[i, j]);
                    AddUnit(data.Actor, data.Platform);
                }
            });
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