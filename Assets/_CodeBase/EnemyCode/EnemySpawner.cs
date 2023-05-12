using System;
using System.Collections.Generic;
using System.Linq;
using _CodeBase.Infrastructure;
using _CodeBase.Infrastructure.Services;
using UnityEngine;

namespace _CodeBase.EnemyCode
{
  public class EnemySpawner : MonoBehaviour
  {
    [SerializeField] private Transform _spawnPointsParent;
    [SerializeField] private Enemy _enemyPrefab;

    private GameState _gameState;
    private List<Transform> _spawnPoints = new List<Transform>();
    private List<Enemy> _enemies = new List<Enemy>();

    private void Awake() => _gameState = ServiceLocator.Get<GameState>();

    private void Start()
    {
      _spawnPoints = _spawnPointsParent.GetComponentsInChildren<Transform>().ToList();
      _spawnPoints.Remove(_spawnPointsParent);
      SpawnEnemies();
    }

    private void OnDestroy() => 
      _enemies.ForEach(enemy => enemy.Died -= OnEnemyDeath);

    private void SpawnEnemies() => _spawnPoints.ForEach(SpawnEnemy);

    private void SpawnEnemy(Transform spawnPoint)
    {
      Enemy enemy = Instantiate(_enemyPrefab, transform);
      enemy.transform.position = spawnPoint.position;
      enemy.Died += OnEnemyDeath;
      _enemies.Add(enemy);
    }

    private void OnEnemyDeath(Enemy enemy)
    {
      enemy.Died -= OnEnemyDeath;
      _enemies.Remove(enemy);
      
      if(_enemies.Count > 0) return;
      _gameState.Win();
    }
  }
}