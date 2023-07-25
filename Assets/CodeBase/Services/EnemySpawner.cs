using System.Collections;
using System.Collections.Generic;
using CodeBase.Databases;
using CodeBase.Infrastructure;
using CodeBase.Units;
using UnityEngine;

namespace CodeBase.Services
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDatabase _enemyDatabase;
        [SerializeField] private WavesDatabase _wavesDatabase;
        [SerializeField] private LevelStaticData _levelStaticData;

        private List<Unit> _spawnedEnemies;

        private IEnumerator Start()
        {
            var prefab = _enemyDatabase.VariantOne;
            int amount = _wavesDatabase.Datas[0].Enemies[0].Amount;
        
            _spawnedEnemies = new(amount);

            for (int i = 0; i < amount; i++)
            {
                yield return new WaitForSeconds(2);
            
                var position = _levelStaticData.EnemyPositions.Random();
                var enemy = Instantiate(prefab, position, Quaternion.identity);
                enemy.SetDestination(_levelStaticData.PlayerBase);

                _spawnedEnemies.Add(enemy);
            }
        }
    }
}