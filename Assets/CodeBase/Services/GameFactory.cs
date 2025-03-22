using System.Collections.Generic;
using Databases;
using Gameplay.Grid;
using Gameplay.Units;
using Services.Infrastructure;
using UI.GameplayWindow;
using UI.WorldSpace;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services
{
    public class GameFactory : IService
    {
        private readonly AssetsProvider _assetsProvider;
        private readonly WindowsService _windowsService;
        private readonly UnitsDatabase _unitsDatabase;
        private readonly LevelDatabase _levelDatabase;
        private readonly Dictionary<string, Object> _cache;
        private readonly IUpdateable _updateable;

        public GameFactory(DatabaseProvider database,
                           AssetsProvider assetsProvider,
                           WindowsService windowsService,
                           IUpdateable updateable)
        {
            _assetsProvider = assetsProvider;
            _windowsService = windowsService;
            _updateable = updateable;
            _unitsDatabase = database.GetDatabase<UnitsDatabase>();
            _levelDatabase = database.GetDatabase<LevelDatabase>();
            _cache = new Dictionary<string, Object>();
        }

        public void CreatePlayerActor(ActorData actorData, Platform platform) =>
            platform.Actor = CreateActor(actorData, platform.transform.position, true);

        public Actor CreateEnemyActor(ActorData data, Vector3 position) => 
            CreateActor(data, position, false);

        private Actor CreateActor(ActorData data, Vector3 position, bool isMy)
        {
            ActorConfig config = _unitsDatabase.ConfigFor(data);
            Actor baseView = Object.Instantiate(config.ViewData.BaseView, position, quaternion.identity);

            CanvasHealthbar healthbar = CreateHealthbar(baseView.transform, isMy);
            ActorRank actorRank = CreateActorRank(baseView.transform, data);
            ActorSkin skin = CreateSkin(config.ViewData.Skin, baseView.transform, healthbar, actorRank);

            baseView.Initialize(skin, data, config.Stats);
            baseView.gameObject.name += Random.Range(0, 100000);

            GameObject shadowPrefab = Load<GameObject>(AssetsPath.ActorShadow);
            Object.Instantiate(shadowPrefab, baseView.transform);
            
            if (!isMy)
                baseView.DisableCollider();

            return baseView;
        }

        public void CreateMergeVFX(Vector3 position) => 
            Object.Instantiate(Load<GameObject>(AssetsPath.MergeVFX), position + Vector3.up, Quaternion.identity);

        private ActorSkin CreateSkin(ActorSkin prefab, Transform parent, CanvasHealthbar healthbar, ActorRank actorRank)
        {
            ActorSkin skin = Object.Instantiate(prefab, parent, false);
            ParticleSystem blood = Object.Instantiate(Load<ParticleSystem>(AssetsPath.BloodFromCrit), skin.transform, false);
            ParticleSystem skull = Object.Instantiate(Load<ParticleSystem>(AssetsPath.DeathSkull), skin.transform, false);
            blood.transform.localPosition = Vector3.up;
            skull.transform.localPosition = Vector3.up / 2;
            skin.Initialize(healthbar, actorRank, blood, skull);
            return skin;
        }

        private CanvasHealthbar CreateHealthbar(Transform target, bool isMy)
        {
            RectTransform rectTransform = _windowsService.Get<GameCanvas>().HealthParent;
            CanvasHealthbar asset = Load<CanvasHealthbar>(AssetsPath.HealthbarCanvas);
            CanvasHealthbar healthbar = Object.Instantiate(asset, rectTransform);
            healthbar.SetColor(isMy);
            healthbar.Init(rectTransform, target);
            return healthbar;
        }

        private ActorRank CreateActorRank(Transform target, ActorData data)
        {
            RectTransform rectTransform = _windowsService.Get<GameCanvas>().RankParent;
            ActorRank asset = Load<ActorRank>(AssetsPath.ActorRankCanvas);
            ActorRank rank = Object.Instantiate(asset, rectTransform);
            rank.Init(rectTransform, target, data);
            return rank;
        }

        public GridView CreateGridView()
        {
            GridView prefab = Load<GridView>(AssetsPath.GridView);
            GridView gridView = Object.Instantiate(prefab, _levelDatabase.GridPosition, quaternion.identity);
            return gridView;
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
    }
}