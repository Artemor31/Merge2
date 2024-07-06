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
        private readonly CameraService _cameraService;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly LevelDatabase _levelDatabase;

        public GameFactory(DatabaseProvider database, AssetsProvider assetsProvider, CameraService cameraService)
        {
            _assetsProvider = assetsProvider;
            _cameraService = cameraService;
            _unitsDatabase = database.GetDatabase<UnitsDatabase>();
            _levelDatabase = database.GetDatabase<LevelDatabase>();
        }

        public Actor CreateActor(UnitId id)
        {
            var data = _unitsDatabase.Units.First(u => u.Id == id);
            var actor = Object.Instantiate(data.Prefab);
            actor.gameObject.name += Random.Range(0, 10000);
            actor.Initialize(data.Level, id);
            CreateHealthbar(actor);
            return actor;
        }

        public Actor CreateActor(UnitId id, Platform platform)
        {
            Actor actor = CreateActor(id);
            actor.transform.position = platform.transform.position;
            return actor;
        }

        private void CreateHealthbar(Actor actor)
        {
            var asset = _assetsProvider.Load<Healthbar>(AssetsPath.Healthbar);
            var healthbar = Object.Instantiate(asset);
            Camera camera = _cameraService.CurrentMainCamera();
            var health = actor.GetComponent<Health>();
            
            healthbar.Initialize(camera, actor, health);
        }

        public void CreateGridView(GridService gridService)
        {
            GridView gridView = Object.Instantiate(_assetsProvider.Load<GridView>(AssetsPath.GridView));
            gridView.Init(gridService);
            gridView.transform.position = _levelDatabase.GridPosition;
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