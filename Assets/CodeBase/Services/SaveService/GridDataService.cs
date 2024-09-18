using System.Collections.Generic;
using Gameplay.LevelItems;
using Gameplay.Units;
using UnityEngine;
using System;
using Data;

namespace Services.SaveService
{
    public class GridDataService : IService
    {
        public IReadOnlyList<Actor> PlayerUnits => GetPlayerUnits();

        private const string GridData = "GridData";
        private readonly Vector2Int GridSize = new(3, 5);
        private readonly SaveService _saveService;
        private GridData _gridData;
        private Platform[,] _platforms;
        private Vector2Int? _selected;

        public GridDataService(SaveService saveService) => _saveService = saveService;
        public ActorData ActorDataAt(int i, int j) => _gridData.UnitIds[i, j];
        public Platform GetDataAt(Vector2Int selected) => _platforms[selected.x, selected.y];

        public void InitPlatforms(Platform[,] platforms)
        {
            _platforms = platforms;
            Restore();
        }
        
        public void Dispose()
        {
            DoForeach((i, j) =>
            {
                if (_platforms[i, j].Actor != null)
                {
                    _platforms[i,j].Actor.Dispose();
                }
            });
            
            _platforms = null;
        }

        public bool HasFreePlatform(out Platform platform)
        {
            platform = FreePlatform();
            return platform != null;
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

        private Platform FreePlatform()
        {
            for (int i = 0; i < _platforms.GetLength(0); i++)
                for (int j = 0; j < _platforms.GetLength(1); j++)
                    if (_platforms[i, j].Free)
                        return _platforms[i, j];

            return null;
        }

        public void Save()
        {
            DoForeach((i, j) =>
            {
                _gridData.UnitIds[i, j] = _platforms[i, j].Busy 
                    ? _platforms[i, j].Actor.Data 
                    : ActorData.None;
            });
            
            _saveService.Save(GridData, _gridData);
        }

        private void Restore()
        {
            _gridData = _saveService.Restore<GridData>(GridData);
            _gridData.UnitIds ??= new ActorData[GridSize.x, GridSize.y];
        }

        private void DoForeach(Action<int, int> action)
        {
            for (int i = 0; i < _platforms.GetLength(0); i++)
                for (int j = 0; j < _platforms.GetLength(1); j++)
                    action.Invoke(i, j);
        }
    }
}