using System.Collections.Generic;
using NaughtyAttributes.Core.DrawerAttributes_SpecialCase;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Database/LevelDatabase", fileName = "LevelDatabase", order = 0)]
    public class LevelDatabase : Database
    {
        public Vector3 GridPosition;
        public Vector3 SpawnerPosition;
        public Vector2 SpawnerSize = new (5,3);
        public float SpawnerDelta = 2;

        public Vector3 StartPlatformPoint;
        public Vector2 DeltaPlatformDistance;

        public IEnumerable<Vector3> GetPositions()
        {
            Vector2 size = SpawnerSize;
            for (int i = 0; i < size.x; i++)
            {
                Vector3 position = SpawnerPosition;
                float delta = SpawnerDelta;
                float currentX = position.x + delta * i;

                for (int j = 0; j < size.y; j++)
                {
                    yield return new Vector3(currentX, 0, position.z + delta * j);
                }
            }
        }
        
        private List<GameObject> _spheres;
        
        [Button]
        public void DrawSpheres()
        {
            _spheres = new List<GameObject>();

            foreach (Vector3 position in GetPositions())
            {
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = position;
                _spheres.Add(sphere);
            }
        }
        
        [Button]
        public void ClearSpheres()
        {
            for (int i = 0; i < _spheres.Count; i++)
            {
                DestroyImmediate(_spheres[i]);
            }
            _spheres = new List<GameObject>();
        }

        private GameObject _spawner;
        [Button]
        public void CreateSpawnPoint()
        {
            _spawner = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _spawner.transform.position = SpawnerPosition;
        }

        [Button]
        public void CollectSpawnPoint()
        {
            SpawnerPosition = _spawner.transform.position;
            DestroyImmediate(_spawner);
            _spawner = null;
        }
    }
}