using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create LevelDatabase", fileName = "LevelDatabase", order = 0)]
    public class LevelDatabase : Database
    {
        public Vector3 GridPosition;
        public Vector3 SpawnerPosition;
        public Vector2 SpawnerSize = new (5,3);
        public float SpawnerDelta = 2;

        public Vector3 StartPlatformPoint;
        public Vector2 DeltaPlatformDistance;
    }
}