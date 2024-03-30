using UnityEngine;

namespace CodeBase.Databases
{
    [CreateAssetMenu(menuName = "Create LevelDatabase", fileName = "LevelDatabase", order = 0)]
    public class LevelDatabase : Database
    {
        public Vector3 GridPosition;
        public Vector3 SpawnerPosition;

        public Vector3 StartPlatformPoint;
        public Vector2 DeltaPlatformDistance;
    }
}