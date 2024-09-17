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

        private const string GridData = "GridData";
        private GridData _gridData;
        private readonly GameFactory _factory;
        private readonly SaveService _saveService;
        private GridRuntimeData[,] _gridRuntimeArray;
        private Vector2Int _gridSize = new(3, 5);
        private Vector2Int? _selected;

        public GridDataService(GameFactory factory, SaveService saveService)
        {
            _factory = factory;
            _saveService = saveService;
        }

        public void SpawnPlatforms()
        {
            Platform[,] platforms =  _factory.CreatePlatforms(_gridSize);
            _gridRuntimeArray = new GridRuntimeData[_gridSize.x, _gridSize.y];
            
            DoForeach((i, j) =>
            {
                _gridRuntimeArray[i, j] = new GridRuntimeData();
                _gridRuntimeArray[i, j].Platform = platforms[i, j];
                _gridRuntimeArray[i, j].Platform.Init(i, j);
                _gridRuntimeArray[i, j].Platform.OnClicked += PlatformOnOnClicked;
                _gridRuntimeArray[i, j].Platform.OnReleased += PlatformOnOnReleased;
                _gridRuntimeArray[i, j].Platform.OnHovered += PlatformOnOnHovered;
            });

            Restore();
        }
        
        public void Dispose()
        {
            DoForeach((i, j) =>
            {
                _gridRuntimeArray[i, j].Platform.OnClicked -= PlatformOnOnClicked;
                _gridRuntimeArray[i, j].Platform.OnReleased -= PlatformOnOnReleased;
                _gridRuntimeArray[i, j].Platform.OnHovered -= PlatformOnOnHovered;
                if (_gridRuntimeArray[i, j].Actor != null)
                {
                    _gridRuntimeArray[i,j].Actor.Dispose();
                }
            });
            
            _gridRuntimeArray = null;
        }
        
        public GridRuntimeData GetDataAt(Vector2Int selected) => _gridRuntimeArray[selected.x, selected.y];
        public bool HasFreePlatform() => FreePlatform() != null;
        public void AddPlayerUnit(ActorConfig config) => AddUnit(_factory.CreateActor(config.Data), FreePlatform());

        private List<Actor> GetPlayerUnits()
        {
            List<Actor> units = new();
            DoForeach((i, j) =>
            {
                if (_gridRuntimeArray[i, j].Busy)
                    units.Add(_gridRuntimeArray[i, j].Actor);
            });

            return units;
        }

        private Platform FreePlatform()
        {
            Platform platform = null;
            DoForeach((i, j) =>
            {
                if (_gridRuntimeArray[i, j].Free)
                    platform = _gridRuntimeArray[i, j].Platform;
            });
            return platform;
        }

        private void AddUnit(Actor actor, Platform platform)
        {
            for (int i = 0; i < _gridRuntimeArray.GetLength(0); i++)
            {
                for (int j = 0; j < _gridRuntimeArray.GetLength(1); j++)
                {
                    if (_gridRuntimeArray[i, j].Platform != platform) continue;
                    _gridRuntimeArray[i, j].Actor = actor;
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
                if (_gridRuntimeArray[i, j].Busy)
                {
                    Actor actor = _gridRuntimeArray[i, j].Actor;
                    unitDatas[i, j] = actor.Data;
                }
                else
                {
                    unitDatas[i, j] = ActorData.None;
                }
            });

            _gridData.UnitIds = unitDatas;
            _saveService.Save(GridData, _gridData);
        }

        private void Restore()
        {
            _gridData = _saveService.Restore<GridData>(GridData);
            
            if (_gridData.UnitIds == null)
            {
                _gridData.UnitIds = new ActorData[_gridSize.x, _gridSize.y];
                return;
            }
            
            DoForeach((i, j) =>
            {
                GridRuntimeData data = _gridRuntimeArray[i, j];
                if (data != null && !_gridData.UnitIds[i, j].Equals(ActorData.None))
                {
                    data.Actor = _factory.CreateActor(_gridData.UnitIds[i, j]);
                    AddUnit(data.Actor, data.Platform);
                }
            });
        }

        private void DoForeach(Action<int, int> action)
        {
            for (int i = 0; i < _gridRuntimeArray.GetLength(0); i++)
                for (int j = 0; j < _gridRuntimeArray.GetLength(1); j++)
                    action.Invoke(i, j);
        }
        
        private void PlatformOnOnHovered(Vector2Int vect) => OnPlatformHovered?.Invoke(_gridRuntimeArray[vect.x, vect.y]);
        private void PlatformOnOnReleased(Vector2Int vect) => OnPlatformReleased?.Invoke(_gridRuntimeArray[vect.x, vect.y]);
        private void PlatformOnOnClicked(Vector2Int vect) => OnPlatformClicked?.Invoke(_gridRuntimeArray[vect.x, vect.y]);
    }
    
    [Serializable]
    public class GridData
    {
        public ActorData[,] UnitIds;
        public GridData(ActorData[,] unitId) => UnitIds = unitId;
        public GridData() => UnitIds = null;
    }
}