using System.Linq;
using CodeBase.Databases;
using CodeBase.Gameplay.Units;
using CodeBase.LevelData;
using UnityEngine;

namespace CodeBase.Services
{
    public class GameFactory : IService
    {
        private readonly AssetsProvider _assetsProvider;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly LevelDatabase _levelDatabase;

        public GameFactory(DatabaseProvider database, AssetsProvider assetsProvider)
        {
            _assetsProvider = assetsProvider;
            _unitsDatabase = database.GetDatabase<UnitsDatabase>();
            _levelDatabase = database.GetDatabase<LevelDatabase>();
        }

        public Actor CreateUnit(UnitId id)
        {
            var unitData = _unitsDatabase.Units.First(u => u.Id == id);
            var unit = Object.Instantiate(unitData.Prefab);
            unit.gameObject.name += Random.Range(0, 10000);
            unit.Initialize(unitData.Level, id);
            return unit;
        }

        public GridView CreateGridView(GridService gridService)
        {
            GridView gridView = Object.Instantiate(_assetsProvider.Load<GridView>(AssetsPath.GridView));
            gridView.Init(gridService);
            gridView.transform.position = _levelDatabase.GridPosition;
            return gridView;
        }

        public Platform[,] CreatePlatforms(Vector2Int size)
        {
            var platforms = new Platform[size.x, size.y];
            Vector2 delta = _levelDatabase.DeltaPlatformDistance;
            Vector3 startPoint = _levelDatabase.StartPlatformPoint;
            Platform platform = _assetsProvider.Load<Platform>(AssetsPath.Platform);

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var clone = Object.Instantiate(platform, startPoint, Quaternion.identity);
                    platforms[i, j] = clone;
                    startPoint += new Vector3(delta.x, 0, 0);
                }

                startPoint = new Vector3(_levelDatabase.StartPlatformPoint.x, startPoint.y, startPoint.z);
                startPoint += new Vector3(0, 0, delta.y);
            }

            return platforms;
        }
    }
}