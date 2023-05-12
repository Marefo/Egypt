using System;
using System.Collections.Generic;
using _CodeBase.EnemyCode;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using _CodeBase.Infrastructure.Services;
using _CodeBase.Logic;
using _CodeBase.ProjectileCode.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.ProjectileCode
{
  public class Projectile : MonoBehaviour
  {
    public event Action<Projectile> Destroyed;
    
    public Vector3 StartDirection { get; private set; }
    public Vector3 ShooterPosition { get; private set; }
    
    [SerializeField] private TriggerListener _triggerListener;
    [SerializeField] private Rigidbody _rigidbody;
    [Space(10)] 
    [SerializeField] private ProjectileSettings _settings;

    private const float ForceMultiplier = 10;
    private const float MinCollisionsDelay = 0.05f;

    private CoroutineService _coroutineService;
    private Collider _lastCollision;
    private Vector3 _initialVelocity;
    private float _lastCollisionTime;
    private GameObject _lastTriggerEnter;

    public void Initialize(Vector3 shooterPosition, Vector3 initialVelocity, CoroutineService coroutineService)
    {
      StartDirection = initialVelocity.normalized;
      ShooterPosition = shooterPosition;
      _coroutineService = coroutineService;
      _rigidbody.velocity = initialVelocity;
      StartCoroutine(_coroutineService.CallWithDelay(Destroy, _settings.LifeTime));
    }

    private void FixedUpdate() => RotateToDirection();

    private void OnEnable() => _triggerListener.Entered += OnTriggerListenerEnter;

    private void OnDisable() => _triggerListener.Entered -= OnTriggerListenerEnter;

    private void OnTriggerListenerEnter(Collider obj)
    {
      if(_lastTriggerEnter == obj.gameObject) return;
      if (obj.TryGetComponent(out Enemy enemy) == false || enemy.Alive == false) return;
      _lastTriggerEnter = obj.gameObject;
      OnEnemyCollide(obj, enemy);
    }

    private void OnCollisionEnter(Collision collision)
    {
      if(collision.collider == _lastCollision && Time.time < _lastCollisionTime + MinCollisionsDelay) return;
      
      _lastCollision = collision.collider;
      _lastCollisionTime = Time.time;
      _lastTriggerEnter = null;
      
      CheckForRicochet(collision);
    }

    private void RotateToDirection()
    {
      if(_rigidbody.velocity != Vector3.zero)
        transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
    }

    private void CheckForRicochet(Collision collision)
    {
      if (collision.gameObject.CompareLayers(_settings.RicochetLayers) == false) return;
      OnRicochetableObjectCollide(collision);
    }

    private void OnRicochetableObjectCollide(Collision collision)
    {
      SpawnRicochetVfx();
      Vector3 collidePoint = collision.GetContact(0).point;
      _rigidbody.velocity = Vector3.zero;
      Vector3 collidedSurfaceNormal = collision.GetContact(0).normal;
      Vector3 reflectedVector = Vector3.Reflect(transform.forward, collidedSurfaceNormal);
      Vector3 direction = reflectedVector.normalized;
      direction.z = 0;
      transform.position = collidePoint;
      transform.rotation = Quaternion.LookRotation(reflectedVector);
      Ricochet(direction);
    }

    private void OnEnemyCollide(Collider obj, Enemy enemy)
    {
      SpawnBloodVfx();
      enemy.OnProjectileHit(obj, _settings.KnockBackForce);
    }

    private void SpawnRicochetVfx()
    {
      Transform ricochetVfx = Instantiate(_settings.RicochetVfx, transform.position, Quaternion.identity).transform;
      ricochetVfx.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -transform.rotation.eulerAngles.y,
        transform.rotation.eulerAngles.z);
    }
    
    private void SpawnBloodVfx()
    {
      Transform bloodVfx = Instantiate(_settings.BloodVfx, transform.position + _settings.BloodOffset, Quaternion.identity).transform;
      bloodVfx.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -transform.rotation.eulerAngles.y,
        transform.rotation.eulerAngles.z);
    }

    private void Ricochet(Vector3 direction) => 
      _rigidbody.AddForce(direction * _settings.Force * ForceMultiplier, ForceMode.Force);

    private void Destroy()
    {
      Destroyed?.Invoke(this);
      Destroy(gameObject);
    }
  }
}