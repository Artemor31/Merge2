﻿using System.Collections.Generic;
using Databases;
using Databases.Data;
using Gameplay.Grid;
using Gameplay.Units;
using Services.Infrastructure;
using Services.Resources;
using UI.GameplayWindow;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services
{
    public class GameFactory : IService
    {
        private readonly AssetsProvider _assetsProvider;
        private readonly CameraService _cameraService;
        private readonly WindowsService _windowsService;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly LevelDatabase _levelDatabase;
        private readonly Dictionary<string, Object> _cache;

        public GameFactory(DatabaseProvider database,
                           AssetsProvider assetsProvider,
                           CameraService cameraService,
                           WindowsService windowsService)
        {
            _assetsProvider = assetsProvider;
            _cameraService = cameraService;
            _windowsService = windowsService;
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

            CanvasHealthbar healthbar = CreateHealthbar(baseView.transform, data.Level);
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

        private ActorSkin CreateSkin(ActorSkin prefab, Transform parent, CanvasHealthbar healthbar)
        {
            ActorSkin skin = Object.Instantiate(prefab, parent, false);
            skin.Initialize(healthbar);
            return skin;
        }

        private CanvasHealthbar CreateHealthbar(Transform target, int level)
        {
            CanvasHealthbar asset = Load<CanvasHealthbar>(AssetsPath.HealthbarCanvas);
            RectTransform rectTransform = _windowsService.Get<GameCanvas>().RectTransform;
            CanvasHealthbar healthbar = Object.Instantiate(asset, rectTransform);
            healthbar.Init(_cameraService, rectTransform, target);
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