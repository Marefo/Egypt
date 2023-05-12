using System;
using _CodeBase.HeroCode;
using _CodeBase.Infrastructure;
using _CodeBase.Infrastructure.Services;
using _CodeBase.ProjectileCode;
using Cinemachine;
using UnityEngine;

namespace _CodeBase.CameraCode
{
  public class CameraStateController : MonoBehaviour
  {
    [SerializeField] private float _followTime;
    [Space(10)]
    [SerializeField] private CinemachineStateDrivenCamera _stateDrivenCamera;
    [SerializeField] private CinemachineVirtualCamera _projectileCamera;
    [SerializeField] private Animator _animator;
    [SerializeField] private HeroThrower _heroThrower;

    private float _transitionDuration => _stateDrivenCamera.m_DefaultBlend.m_Time;
    
    private readonly int _followProjectileHash = Animator.StringToHash("FollowProjectile");
    
    private InputService _inputService;
    private CoroutineService _coroutineService;
    private Projectile _lastProjectile;
    private Coroutine _stopFollowingCoroutine;
    private Coroutine _enableInputCoroutine;

    private void Awake()
    {
      _coroutineService = ServiceLocator.Get<CoroutineService>();
      _inputService = ServiceLocator.Get<InputService>();
    }

    private void OnEnable()
    {
      _heroThrower.Throwed += OnThrow;
    }

    private void OnDisable()
    {
      _heroThrower.Throwed -= OnThrow;
    }

    private void OnDestroy()
    {
      if (_lastProjectile != null)
        _lastProjectile.Destroyed -= OnProjectileDestroy;
    }

    private void OnThrow(Projectile projectile)
    {
      if(_stopFollowingCoroutine != null)
        StopCoroutine(_stopFollowingCoroutine);
      if(_enableInputCoroutine != null)
        StopCoroutine(_enableInputCoroutine);
      
      _lastProjectile = projectile;
      projectile.Destroyed += OnProjectileDestroy;
      
      _projectileCamera.Follow = projectile.transform;
      StartFollowingProjectile();
      
      _stopFollowingCoroutine = StartCoroutine(_coroutineService.CallWithDelay(StopFollowingProjectile, _followTime));
      StartCoroutine(_coroutineService.CallWithDelayUnscaled(_inputService.Disable, _transitionDuration));
    }

    private void OnProjectileDestroy(Projectile projectile)
    {
      projectile.Destroyed -= OnProjectileDestroy;
      StopFollowingProjectile();
    }

    private void StartFollowingProjectile() => _animator.SetBool(_followProjectileHash, true);
    
    private void StopFollowingProjectile()
    {
      _animator.SetBool(_followProjectileHash, false);
      _enableInputCoroutine = StartCoroutine(_coroutineService.CallWithDelayUnscaled(_inputService.Enable, _transitionDuration));
    }
  }
}