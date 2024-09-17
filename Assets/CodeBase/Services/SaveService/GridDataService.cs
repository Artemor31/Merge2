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
        public event Action<Platform> OnPlatformClicked;
        public event Action<Platform> OnPlatformReleased;
        public event Action<Platform> OnPlatformHovered;
        
        public IReadOnlyList<Actor> PlayerUnits => GetPlayerUnits();

        private const string GridData = "GridData";
        private GridData _gridData;
        private readonly GameFactory _factory;
        private readonly SaveService _saveService;
        private Platform[,] _platforms;
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
            _platforms = new Platform[_gridSize.x, _gridSize.y];
            
            DoForeach((i, j) =>
            {
                _platforms[i, j] = platforms[i, j];
                _platforms[i, j].Init(i, j);
                _platforms[i, j].OnClicked += PlatformOnOnClicked;
                _platforms[i, j].OnReleased += PlatformOnOnReleased;
                _platforms[i, j].OnHovered += PlatformOnOnHovered;
            });

            Restore();
        }
        
        public void Dispose()
        {
            DoForeach((i, j) =>
            {
                _platforms[i, j].OnClicked -= PlatformOnOnClicked;
                _platforms[i, j].OnReleased -= PlatformOnOnReleased;
                _platforms[i, j].OnHovered -= PlatformOnOnHovered;
                if (_platforms[i, j].Actor != null)
                {
                    _platforms[i,j].Actor.Dispose();
                }
            });
            
            _platforms = null;
        }
        
        public Platform GetDataAt(Vector2Int selected) => _platforms[selected.x, selected.y];
        public bool HasFreePlatform() => FreePlatform() != null;
        public void AddPlayerUnit(ActorConfig config) => AddUnit(_factory.CreateActor(config.Data), FreePlatform());

        private List<Actor> GetPlayerUnits()
        {
            List<Actor> units = new();
            DoForeach((i, j) =>
            {
                if (_platforms[i, j].Busy)
                    units.Add(_platforms[i, j].Actor);
            });

            return units;
        }

        private Platform FreePlatform()
        {
            Platform platform = null;
            DoForeach((i, j) =>
            {
                if (_platforms[i, j].Free)
                    platform = _platforms[i, j];
            });
            return platform;
        }

        private void AddUnit(Actor actor, Platform platform)
        {
            for (int i = 0; i < _platforms.GetLength(0); i++)
            {
                for (int j = 0; j < _platforms.GetLength(1); j++)
                {
                    if (_platforms[i, j] != platform) continue;
                    _platforms[i, j].Actor = actor;
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
                if (_platforms[i, j].Busy)
                {
                    Actor actor = _platforms[i, j].Actor;
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
                Platform platform = _platforms[i, j];
                if (platform != null && !_gridData.UnitIds[i, j].Equals(ActorData.None))
                {
                    platform.Actor = _factory.CreateActor(_gridData.UnitIds[i, j]);
                    AddUnit(platform.Actor, platform);
                }
            });
        }

        private void DoForeach(Action<int, int> action)
        {
            for (int i = 0; i < _platforms.GetLength(0); i++)
                for (int j = 0; j < _platforms.GetLength(1); j++)
                    action.Invoke(i, j);
        }
        
        private void PlatformOnOnHovered(Vector2Int vect) => OnPlatformHovered?.Invoke(_platforms[vect.x, vect.y]);
        private void PlatformOnOnReleased(Vector2Int vect) => OnPlatformReleased?.Invoke(_platforms[vect.x, vect.y]);
        private void PlatformOnOnClicked(Vector2Int vect) => OnPlatformClicked?.Invoke(_platforms[vect.x, vect.y]);
    }
    
    [Serializable]
    public class GridData
    {
        public ActorData[,] UnitIds;
        public GridData(ActorData[,] unitId) => UnitIds = unitId;
        public GridData() => UnitIds = null;
    }
}