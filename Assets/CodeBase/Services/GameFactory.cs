using System.Linq;
using Databases;
using Gameplay.Units;
using Gameplay.Units.Behaviours;
using LevelData;
using UnityEngine;

namespace Services
{
    
    public class GameFactory : IService
    {
        private readonly AssetsProvider _assetsProvider;
        private readonly CameraService _cameraService;
        private readonly IUpdateable _updateable;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly LevelDatabase _levelDatabase;

        public GameFactory(DatabaseProvider database,
                           AssetsProvider assetsProvider,
                           CameraService cameraService,
                           IUpdateable updateable)
        {
            _assetsProvider = assetsProvider;
            _cameraService = cameraService;
            _updateable = updateable;
            _unitsDatabase = database.GetDatabase<UnitsDatabase>();
            _levelDatabase = database.GetDatabase<LevelDatabase>();
        }

        public Actor CreateActor(Race race, Mastery mastery, int level)
        {
            ActorConfig config = _unitsDatabase.Units.First(u => u.Race == race && u.Mastery == mastery && u.Level == level);
            GameObject view = Object.Instantiate(config.Prefab);
            Actor instance = Object.Instantiate(_assetsProvider.Load<Actor>(AssetsPath.SimpleMeleeUnit));
            instance.Initialize(config.Level, view, _updateable, race, mastery);
            CreateHealthbar(instance);
            return instance;
        }

        public Actor CreateActor(Race race, Mastery mastery, int level, Platform platform)
        {
            Actor actor = CreateActor(race, mastery, level);
            actor.transform.position = platform.transform.position;
            return actor;
        }

        public Actor CreateActor(ActorConfig config) => CreateActor(config.Race, config.Mastery, config.Level);

        private void CreateHealthbar(Actor actor)
        {
            Healthbar asset = _assetsProvider.Load<Healthbar>(AssetsPath.Healthbar);
            Healthbar healthbar = Object.Instantiate(asset);
            Camera camera = _cameraService.CurrentMainCamera();
            Health health = actor.GetComponent<Health>();
            
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