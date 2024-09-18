using Data;
using Databases;
using Gameplay.LevelItems;
using Gameplay.Units;
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

        public Actor CreateActor(ActorData data, Vector3 position)
        {
            var actor = CreateActor(data);
            actor.transform.position = position;
            return actor;
        }
        
        public Actor CreateActor(ActorData data)
        {
            ActorConfig config = _unitsDatabase.ConfigFor(data);
            GameObject view = Object.Instantiate(config.ViewData.Prefab);
            Actor instance = Object.Instantiate(config.ViewData.BaseView);
            instance.Initialize(view, _updateable, data, config.Stats);
            instance.gameObject.name += Random.Range(0, 100000); 
            CreateHealthbar(instance);
            return instance;
        }
        
        private void CreateHealthbar(Actor actor)
        {
            Healthbar asset = _assetsProvider.Load<Healthbar>(AssetsPath.Healthbar);
            Healthbar healthbar = Object.Instantiate(asset);
            Camera camera = _cameraService.CurrentMainCamera();
            healthbar.Initialize(camera, actor, _updateable);
        }

        public void CreateGridView()
        {
            GridView gridView = Object.Instantiate(_assetsProvider.Load<GridView>(AssetsPath.GridView));
            gridView.Init();
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