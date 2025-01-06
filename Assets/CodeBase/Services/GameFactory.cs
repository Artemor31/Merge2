using System.Collections.Generic;
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
        private readonly Dictionary<string, Object> _cache;

        public GameFactory(DatabaseProvider database, AssetsProvider assetsProvider, CameraService cameraService)
        {
            _assetsProvider = assetsProvider;
            _cameraService = cameraService;
            _unitsDatabase = database.GetDatabase<UnitsDatabase>();
            _levelDatabase = database.GetDatabase<LevelDatabase>();
            _cache = new Dictionary<string, Object>();
        }

        public void CreatePlayerActor(ActorData actorData, Platform platform) =>
            platform.Actor = CreateActor(actorData, platform.transform.position);

        public Actor CreateEnemyActor(ActorData data, Vector3 position) => CreateActor(data, position);

        private Actor CreateActor(ActorData data, Vector3 position)
        {
            ActorConfig config = _unitsDatabase.ConfigFor(data);
            Actor baseView = Object.Instantiate(config.ViewData.BaseView, position, quaternion.identity);

            Healthbar healthbar = CreateHealthbar(baseView.transform, data.Level);
            ActorSkin skin = CreateSkin(config.ViewData.Skin, baseView.transform, healthbar);

            baseView.Initialize(skin, data, config.Stats);
            baseView.gameObject.name += Random.Range(0, 100000);

            GameObject shadowPrefab = Load<GameObject>(AssetsPath.ActorShadow);
            Object.Instantiate(shadowPrefab, baseView.transform);

            return baseView;
        }

        private T Load<T>(string path) where T : Object
        {
            if (_cache.TryGetValue(path, out Object value))
            {
                return (T)value;
            }

            T prefab = _assetsProvider.Load<T>(path);
            _cache.Add(path, prefab);
            return prefab;
        }

        private ActorSkin CreateSkin(ActorSkin prefab, Transform parent, Healthbar healthbar)
        {
            ActorSkin skin = Object.Instantiate(prefab, parent, false);
            skin.Initialize(healthbar);
            return skin;
        }

        private Healthbar CreateHealthbar(Transform target, int level)
        {
            Healthbar asset = Load<Healthbar>(AssetsPath.Healthbar);
            Healthbar healthbar = Object.Instantiate(asset);
            Camera camera = _cameraService.CurrentCamera();
            healthbar.Initialize(camera, target, level);
            return healthbar;
        }

        public GridView CreateGridView()
        {
            GridView prefab = Load<GridView>(AssetsPath.GridView);
            GridView gridView = Object.Instantiate(prefab);
            gridView.Init();
            gridView.transform.position = _levelDatabase.GridPosition;
            return gridView;
        }
    }
}