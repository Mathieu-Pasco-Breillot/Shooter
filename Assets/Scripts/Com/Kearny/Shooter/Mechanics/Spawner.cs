using System;
using UnityEngine;

namespace Com.Kearny.Shooter.Mechanics
{
    public class Spawner : MonoBehaviour
    {
        public Wave[] waves;
        public Enemy.Enemy enemy;

        private Wave _currentWave;
        private int _currentWaveNumber;

        private int _enemiesRemainingToSpawn;
        private int _enemiesRemainingAlive;
        private float _nextSpawnTime;

        private void Start()
        {
            NextWave();
        }

        private void Update()
        {
            if (_enemiesRemainingToSpawn > 0 && Time.time > _nextSpawnTime)
            {
                _enemiesRemainingToSpawn--;
                _nextSpawnTime = Time.time + _currentWave.timeBetweenSpawns;

                if (enemy != null)
                {
                    var spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity);
                    spawnedEnemy.OnDeath += OnEnemyDeath;
                }
            }
        }

        private void OnEnemyDeath()
        {
            _enemiesRemainingAlive--;

            if (_enemiesRemainingAlive == 0)
            {
                NextWave();
            }
        }

        private void NextWave()
        {
            _currentWaveNumber++;

            if (_currentWaveNumber - 1 >= waves.Length) return;
            
            _currentWave = waves[_currentWaveNumber - 1];

            _enemiesRemainingToSpawn = _currentWave.enemyCount;
            _enemiesRemainingAlive = _enemiesRemainingToSpawn;
        }

        [System.Serializable]
        public class Wave
        {
            public int enemyCount;
            public float timeBetweenSpawns;
        }
    }
}