using Databases;
using Databases.Data;
using Gameplay.Grid;
using Gameplay.Units;
using Gameplay.Units.Healths;
using Services.Infrastructure;
using Services.Resources;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services
{
    
    public class GameFactory : IService
    {
        private readonly AssetsProvider _assetsProvider;
        private readonly CameraService _cameraService;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly LevelDatabase _levelDatabase;

        public GameFactory(DatabaseProvider database,
                           AssetsProvider assetsProvider,
                           CameraService cameraService)
        {
            _assetsProvider = assetsProvider;
            _cameraService = cameraService;
            _unitsDatabase = database.GetDatabase<UnitsDatabase>();
            _levelDatabase = database.GetDatabase<LevelDatabase>();
        }

        public void CreatePlayerActor(ActorData actorData, Platform platform) => platform.Actor = CreateActor(actorData, platform.transform.position);
        public Actor CreateEnemyActor(ActorData data, Vector3 position) => CreateActor(data, position);

        private Actor CreateActor(ActorData data, Vector3 position)
        {
            ActorConfig config = _unitsDatabase.ConfigFor(data);
            Actor baseView = Object.Instantiate(config.ViewData.BaseView, position, quaternion.identity);
            
            Healthbar healthbar = CreateHealthbar(baseView.transform, data.Level);
            ActorSkin skin = CreateSkin(config.ViewData.Skin, baseView.transform, healthbar);

            baseView.Initialize(skin, data, config.Stats);
            baseView.gameObject.name += Random.Range(0, 100000);
            return baseView;
        }

        private ActorSkin CreateSkin(ActorSkin prefab, Transform parent, Healthbar healthbar)
        {
            ActorSkin skin = Object.Instantiate(prefab, parent, false);
            skin.Initialize(healthbar);
            return skin;
        }
        
        private Healthbar CreateHealthbar(Transform target, int level)
        {
            Healthbar asset = _assetsProvider.Load<Healthbar>(AssetsPath.Healthbar);
            Healthbar healthbar = Object.Instantiate(asset);
            Camera camera = _cameraService.CurrentCamera();
            healthbar.Initialize(camera, target, level);
            return healthbar;
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