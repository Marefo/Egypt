using System;
using _CodeBase.HeroCode.Data;
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
    
    public int CurrentProjectilesNumber { get; private set; }
    
    [SerializeField] private CoroutineService _coroutineService; 
    [Space(10)]
    [SerializeField] private Projectile _projectilePrefab;
    [Space(10)]
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private GameObject _projectileInHandModel;
    [Space(10)]
    [SerializeField] private HeroAimer _aimer;
    [Space(10)] 
    [SerializeField] private HeroThrowerSettings _settings;

    private void Start() => ChangeProjectilesNumber(_settings.ProjectilesNumber);

    private void OnEnable() => _aimer.AimingFinished += OnAimingFinish;
    private void OnDisable() => _aimer.AimingFinished -= OnAimingFinish;

    private void OnAimingFinish(Vector3 initialVelocity, Vector3 shooterPosition) => 
      Throw(initialVelocity, shooterPosition);

    public void TryThrowWithoutProjectiles() => TriedThrowWithoutProjectiles?.Invoke();

    private void Throw(Vector3 initialVelocity, Vector3 shooterPosition)
    {
      if(CurrentProjectilesNumber == 0) return;
      ChangeProjectilesNumber(CurrentProjectilesNumber - 1);
      ChangeInHandShurikenVisibility(false);
      
      Vector3 createPosition = _projectileSpawnPoint.position;
      createPosition.z = shooterPosition.z;
      Vector3 spawnPoint = _projectileSpawnPoint.position;
      spawnPoint.z = shooterPosition.z;
      
      Projectile projectile = Instantiate(_projectilePrefab, spawnPoint, Quaternion.identity);
      projectile.Initialize(shooterPosition, initialVelocity, _coroutineService);
    }

    private void ChangeProjectilesNumber(int newNumber)
    {
      CurrentProjectilesNumber = newNumber;
      ProjectilesNumberChanged?.Invoke(newNumber);
    }
    
    private void ChangeInHandShurikenVisibility(bool show) => _projectileInHandModel.SetActive(show);
  }
}