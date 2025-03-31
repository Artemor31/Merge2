using System.Collections.Generic;
using System.Linq;
using Databases;
using Gameplay.Grid;
using Gameplay.Units;
using Infrastructure;
using Services.Infrastructure;
using Services.ProgressData;
using UnityEngine;

namespace Services.DataServices
{
    public class GridDataService : IService
    {
        private const string StoryGridData = "StoryGridData";
        
        public List<Actor> PlayerUnits => GetPlayerUnits();
        public Vector2Int GridSize => new(3, 5);

        private readonly SaveService _saveService;
        private GridProgress _gridProgress;
        private List<Platform> _platforms;

        public GridDataService(SaveService saveService) => _saveService = saveService;
        public ActorData ActorDataAt(int index) => _gridProgress.UnitIds.Count > index ? _gridProgress.UnitIds[index] : ActorData.None;
        public Platform GetPlatform(int index) => _platforms[index];
        public bool HasFreePlatform() => _platforms.FirstOrDefault(p => p.Free) != null;
        public Platform RandomPlatform() => _platforms.Where(platform => platform.Free).Random();
        
        public void RestoreData(List<Platform> platforms)
        {
            _platforms = platforms;
            _gridProgress = _saveService.Restore<GridProgress>(StoryGridData);
            _gridProgress.UnitIds ??= new List<ActorData>(_platforms.Count);
            
            foreach (var unused in _platforms)
                _gridProgress.UnitIds.Add(ActorData.None);
        }

        public void Save()
        {
            for (int i = 0; i < _platforms.Count; i++)
            {
                Platform platform = _platforms[i];
                _gridProgress.UnitIds[i] = platform.Busy ? platform.Actor.Data : ActorData.None;
            }
            
            _saveService.Save(StoryGridData, _gridProgress);
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