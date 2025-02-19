using System.Collections.Generic;
using System.Linq;
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
        public Vector2Int GridSize => new(_persistantDataService.Rows, 5);
        
        private readonly SaveService _saveService;
        private readonly PersistantDataService _persistantDataService;
        private GridProgress _gridProgress;
        private List<Platform> _platforms;

        public GridDataService(SaveService saveService, PersistantDataService persistantDataService)
        {
            _saveService = saveService;
            _persistantDataService = persistantDataService;
        }

        public ActorData ActorDataAt(int index) => _gridProgress.UnitIds.Count > index
            ? _gridProgress.UnitIds[index]
            : ActorData.None;
        
        public Platform GetPlatform(int index) => _platforms[index];

        public void RestoreData(List<Platform> platforms)
        {
            _platforms = platforms;
            _gridProgress = _saveService.Restore<GridProgress>(SavePath);
            _gridProgress.UnitIds ??= new List<ActorData>(_platforms.Count);
            
            foreach (var unused in _platforms)
                _gridProgress.UnitIds.Add(ActorData.None);
        }

        public bool TryGetFreePlatform(out Platform platform)
        {
            platform = _platforms.FirstOrDefault(p => p.Free);
            return platform != null;
        }

        public Platform RandomPlatform() => 
            _platforms.Where(platform => platform.Free).Random();

        public void Save()
        {
            for (int i = 0; i < _platforms.Count; i++)
            {
                Platform platform = _platforms[i];
                _gridProgress.UnitIds[i] = platform.Busy ? platform.Actor.Data : ActorData.None;
            }
            
            _saveService.Save(SavePath, _gridProgress);
        }

        public void Reset()
        {
            _gridProgress = new GridProgress();
            _saveService.Save(SavePath, _gridProgress);
        }

        private List<Actor> GetPlayerUnits() => _platforms.Where(platform => platform.Busy)
                                                          .Select(platform => platform.Actor)
                                                          .ToList();

        public void Dispose()
        {
            foreach (Actor actor in PlayerUnits)
            {
                actor.Disable();
            }
        }
    }
}