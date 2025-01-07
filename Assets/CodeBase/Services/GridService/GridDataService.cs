using System;
using System.Collections.Generic;
using Databases.Data;
using Gameplay.Grid;
using Gameplay.Units;
using Infrastructure;
using Services.Infrastructure;
using Services.SaveProgress;
using UnityEngine;

namespace Services.GridService
{
    public class GridDataService : IService
    {
        private const string SavePath = "GridData";
        
        public List<Actor> PlayerUnits => GetPlayerUnits();
        public Vector2Int GridSize { get; } = new(3, 5);
        
        private readonly SaveService _saveService;
        private GridProgress _gridProgress;
        private Platform[,] _platforms;

        public GridDataService(SaveService saveService) => _saveService = saveService;
        public ActorData ActorDataAt(int i, int j) => _gridProgress.UnitIds[i, j];
        public Platform GetDataAt(Vector2Int selected) => _platforms[selected.x, selected.y];

        public void InitPlatforms(Platform[,] platforms)
        {
            _platforms = platforms;
            _gridProgress = _saveService.Restore<GridProgress>(SavePath);
            _gridProgress.UnitIds ??= new ActorData[GridSize.x, GridSize.y];
        }
        
        public bool HasFreePlatform(out Platform platform)
        {
            for (var i = 0; i < _platforms.GetLength(0); i++)
            {
                for (var j = 0; j < _platforms.GetLength(1); j++)
                {
                    if (_platforms[i, j].Free)
                    {
                        platform = _platforms[i, j];
                        return true;
                    }
                }
            }

            platform = null;
            return false;
        }
        
        public Platform RandomPlatform()
        {

            List<Platform> platforms = new();
            for (var i = 0; i < _platforms.GetLength(0); i++)
            {
                for (var j = 0; j < _platforms.GetLength(1); j++)
                {
                    if (_platforms[i, j].Free)
                    {
                        platforms.Add(_platforms[i, j]);
                    }
                }
            }

            return platforms.Random();
        }

        public void Save()
        {
            DoForeach((i, j) =>
            {
                _gridProgress.UnitIds[i, j] = _platforms[i, j].Busy ? _platforms[i, j].Actor.Data : ActorData.None;
            });
            
            _saveService.Save(SavePath, _gridProgress);
        }

        public void Reset()
        {
            _gridProgress = new GridProgress();
            _saveService.Save(SavePath, _gridProgress);
        }

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

        private void DoForeach(Action<int, int> action)
        {
            for (int i = 0; i < _platforms.GetLength(0); i++)
                for (int j = 0; j < _platforms.GetLength(1); j++)
                    action.Invoke(i, j);
        }
    }
}