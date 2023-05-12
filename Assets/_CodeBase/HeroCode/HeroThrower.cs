using System;
using System.Collections.Generic;
using _CodeBase.HeroCode.Data;
using _CodeBase.Infrastructure;
using _CodeBase.Infrastructure.Services;
using _CodeBase.ProjectileCode;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.HeroCode
{
  public class HeroThrower : MonoBehaviour
  {
    public event Action TriedThrowWithoutProjectiles;
    public event Action<int> ProjectilesNumberChanged;
    public event Action<Projectile> Throwed; 

    public int CurrentProjectilesAmount { get; private set; }
    
    [Space(10)]
    [SerializeField] private Projectile _projectilePrefab;
    [Space(10)]
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private GameObject _projectileInHandModel;
    [Space(10)]
    [SerializeField] private HeroAimer _aimer;
    [Space(10)] 
    [SerializeField] private HeroThrowerSettings _settings;

    private GameState _gameState; 
    private CoroutineService _coroutineService; 
    private List<Projectile> _projectiles = new List<Projectile>();

    private void Awake()
    {
      _gameState = ServiceLocator.Get<GameState>();
      _coroutineService = ServiceLocator.Get<CoroutineService>();
    }

    private void Start() => ChangeProjectilesNumber(_settings.ProjectilesNumber);

    private void OnEnable() => _aimer.AimingFinished += OnAimingFinish;
    private void OnDisable() => _aimer.AimingFinished -= OnAimingFinish;

    private void OnDestroy() => 
      _projectiles.ForEach(projectile => projectile.Destroyed -= OnProjectileDestroy);

    private void OnAimingFinish(Vector3 initialVelocity, Vector3 shooterPosition) => 
      Throw(initialVelocity, shooterPosition);

    public void TryThrowWithoutProjectiles() => TriedThrowWithoutProjectiles?.Invoke();

    private void Throw(Vector3 initialVelocity, Vector3 shooterPosition)
    {
      if(CurrentProjectilesAmount == 0) return;
      ChangeProjectilesNumber(CurrentProjectilesAmount - 1);
      ChangeInHandShurikenVisibility(false);
      
      Vector3 createPosition = _projectileSpawnPoint.position;
      createPosition.z = shooterPosition.z;
      Vector3 spawnPoint = _projectileSpawnPoint.position;
      spawnPoint.z = shooterPosition.z;
      
      Projectile projectile = Instantiate(_projectilePrefab, spawnPoint, Quaternion.identity);
      projectile.Initialize(shooterPosition, initialVelocity, _coroutineService);
      Throwed?.Invoke(projectile);

      projectile.Destroyed += OnProjectileDestroy;
      _projectiles.Add(projectile);
    }

    private void OnProjectileDestroy(Projectile obj)
    {
      obj.Destroyed -= OnProjectileDestroy;
      
      if(CurrentProjectilesAmount > 0) return;
      _gameState.Lose();
    }

    private void ChangeProjectilesNumber(int newNumber)
    {
      CurrentProjectilesAmount = newNumber;
      ProjectilesNumberChanged?.Invoke(newNumber);
    }
    
    private void ChangeInHandShurikenVisibility(bool show) => _projectileInHandModel.SetActive(show);
  }
}