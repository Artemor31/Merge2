using System;
using System.Collections.Generic;
using Databases.Data;
using Gameplay.Grid;
using Gameplay.Units;
using Services.Infrastructure;
using UnityEngine;

namespace Services.GridService
{
    public class GridDataService : IService
    {
        public ICollection<Actor> PlayerUnits => GetPlayerUnits();
        
        private const string SavePath = "GridData";

        private readonly Vector2Int _gridSize = new(3, 5);
        private readonly SaveService _saveService;
        private GridData _gridData;
        private Platform[,] _platforms;

        public GridDataService(SaveService saveService) => _saveService = saveService;
        public ActorData ActorDataAt(int i, int j) => _gridData.UnitIds[i, j];
        public Platform GetDataAt(Vector2Int selected) => _platforms[selected.x, selected.y];

        public void InitPlatforms(Platform[,] platforms)
        {
            _platforms = platforms;
            _gridData = _saveService.Restore<GridData>(SavePath);
            _gridData.UnitIds ??= new ActorData[_gridSize.x, _gridSize.y];
        }
        
        public void Dispose()
        {
            DoForeach((i, j) =>
            {
                if (_platforms[i, j].Actor != null) 
                    _platforms[i,j].Actor.Dispose();
            });
            
            _platforms = null;
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

        public void Save()
        {
            DoForeach((i, j) =>
            {
                _gridData.UnitIds[i, j] = _platforms[i, j].Busy ? _platforms[i, j].Actor.Data : ActorData.None;
            });
            
            _saveService.Save(SavePath, _gridData);
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