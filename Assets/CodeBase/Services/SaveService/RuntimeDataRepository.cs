using System;
using System.Collections.Generic;
using CodeBase.Databases;
using CodeBase.Gameplay.Units;
using CodeBase.LevelData;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Services.SaveService
{
    public class PlayerDataProvider : IService
    {
    }

    public class GridRepository
    {
        public void Save(GridData saveData)
        {
            string serializeObject = JsonConvert.SerializeObject(saveData);
            PlayerPrefs.SetString("GridData", serializeObject);
        }

        public GridData Restore()
        {
            string json = PlayerPrefs.GetString("GridData", null);
            if (json == null)
                return null;

            return JsonConvert.DeserializeObject<GridData>(json);
        }

        [Serializable]
        public class GridData
        {
            public UnitId[,] UnitIds;
            public GridData(UnitId[,] unitId) => UnitIds = unitId;
        }
    }

    public class RuntimeDataRepository : IService
    {
        private readonly GameFactory _gameFactory;
        public event Action<int> OnMoneyChanged;
        public event Action<GridRuntimeData> OnPlatformClicked;
        public event Action<GridRuntimeData> OnPlatformReleased;
        public event Action<GridRuntimeData> OnPlatformHovered;

        public int Wave => _progress.Wave;
        public int Money => _progress.Money;
        public Vector2Int GridSize = new(3, 5);
        public List<Actor> EnemyUnits { get; }

        private readonly GridRuntimeData[,] _gridData;
        private readonly PlayerProgress _progress;
        private Vector2Int? _selected;
        private readonly GridRepository _gridRepo;

        public RuntimeDataRepository(GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _progress = new PlayerProgress();
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

            GridRepository.GridData restore = _gridRepo.Restore();
            if (restore?.UnitIds != null)
            {
                DoForeach((i, j) =>
                {
                    var data = _gridData[i, j];
                    var id = restore.UnitIds[i, j];
                    if (data != null && id != UnitId.None)
                    {
                        data.Actor = _gameFactory.CreateActor(id, data.Platform);
                        AddPlayerUnit(data.Actor, data.Platform);
                    }
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

        public void NextLevel()
        {
            _progress.Wave++;
            Save();
        }

        public void Save()
        {
            var ids = new UnitId[GridSize.x, GridSize.y];
            DoForeach((i, j) => ids[i, j] = _gridData[i, j].Busy 
                ? _gridData[i, j].Actor.Id 
                : UnitId.None);
            
            _gridRepo.Save(new GridRepository.GridData(ids));
            _progress.Save();
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