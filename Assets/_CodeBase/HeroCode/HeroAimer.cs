using System;
using System.Collections.Generic;
using _CodeBase.Data;
using _CodeBase.HeroCode.Data;
using _CodeBase.Infrastructure.Services;
using _CodeBase.Logic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Range = _CodeBase.Data.Range;

namespace _CodeBase.HeroCode
{
  public class HeroAimer : MonoBehaviour
  {
    public event Action<Vector3, Vector3> AimingFinished;
    public event Action AimStarted;
    public event Action AimFinished;

    public Vector2 InputForce => _inputForce;

    [SerializeField] private Transform _aimStartPoint;
    [SerializeField] private Transform _aimTopPoint;
    [SerializeField] private Transform _aimTargetPoint;
    [Space(10)]
    [SerializeField] private Transform _rigAimTarget;
    [SerializeField] private Transform _aimPlane;
    [SerializeField] private LineRenderer _aimLine;
    [Space(10)] 
    [SerializeField] private LayerMask _collideLayers;
    [Space(10)] 
    [SerializeField] private Rig _rig;
    [SerializeField] private RotatorToTarget _rotator;
    [SerializeField] private HeroThrower _thrower;
    [SerializeField] private HeroAnimator _animator;
    [Space(10)] 
    [SerializeField] private HeroAimSettings _settings;

    private float _gravity => Physics.gravity.y;
    
    private InputService _inputService;
    private CoroutineService _coroutineService;
    private Camera _mainCamera;
    private Vector2 _inputForce;
    private bool _isTouching;
    private Vector2 _touchStartPosition;
    private TweenerCore<float, float, FloatOptions> _disableIKTween;
    private TweenerCore<float, float, FloatOptions> _enableIKTween;
    private Coroutine _finishAimingCoroutine;
    private Vector3 _shootDirection;

    private void Awake()
    {
      _inputService = ServiceLocator.Get<InputService>();
      _coroutineService = ServiceLocator.Get<CoroutineService>();
      _mainCamera = Camera.main;
      _rotator.Disable();
      ResetAimLine();
      ChangeAimTargetVisibility(false);
      DisableIK();
    }

    private void OnEnable()
    {
      _inputService.TouchEntered += StartAiming;
      _inputService.TouchCanceled += FinishAiming;
      _inputService.Disabled += ResetAiming;
    }

    private void OnDisable()
    {
      _inputService.TouchEntered -= StartAiming;
      _inputService.TouchCanceled -= FinishAiming;
      _inputService.Disabled -= ResetAiming;
    }

    private void FixedUpdate() => TakeAim();

    public void ResetAiming()
    {
      if(_finishAimingCoroutine != null)
        StopCoroutine(_finishAimingCoroutine);
      
      _isTouching = false;
      ChangeAimTargetVisibility(false);
      ResetAimLine();
      DisableIKFast();
      _rotator.Disable();
      _rotator.ResetRotation();
      _animator.ChangeAimingState(false);
    }
    
    private void StartAiming()
    {
      if (_thrower.CurrentProjectilesAmount == 0)
      {
        _thrower.TryThrowWithoutProjectiles();
        return;
      }
      
      if(_finishAimingCoroutine != null)
        StopCoroutine(_finishAimingCoroutine);
      
      _isTouching = true;
      ChangeAimTargetVisibility(true);
      _touchStartPosition = _inputService.TouchStartPosition;
      EnableIK();
      _rotator.Enable();
      _animator.ChangeAimingState(true);
      AimStarted?.Invoke();
    }

    private void FinishAiming()
    {
      if(_isTouching == false) return;
      
      if(_finishAimingCoroutine != null)
        StopCoroutine(_finishAimingCoroutine);
      
      EnableIKFast();
      _isTouching = false;
      ChangeAimTargetVisibility(false);
      ResetAimLine();
      Vector3 initialVelocity = CalculateLaunchData().InitialVelocity;
      initialVelocity.z = transform.position.z;
      AimingFinished?.Invoke(initialVelocity, transform.position);
      AimFinished?.Invoke();
      _finishAimingCoroutine = StartCoroutine(_coroutineService.CallWithDelay(OnFinishAiming, _settings.ResetAimDelay));
    }

    private void OnFinishAiming()
    {
      DisableIK();
      _rotator.Disable();
      _rotator.ResetRotation();
      _animator.ChangeAimingState(false);
    }

    private void TakeAim()
    {
      if(_isTouching == false) return;
      HandleInput();
      UpdateAimPrediction();
      DrawPath();
      AnimateAimLine();
    }

    private void HandleInput()
    {
      Vector3 touchStartPosition = _touchStartPosition;
      touchStartPosition.z = Mathf.Abs(_mainCamera.transform.position.z);
      touchStartPosition = _mainCamera.ScreenToWorldPoint(touchStartPosition);
      Vector3 currentTouchPosition = _inputService.TouchPosition;
      currentTouchPosition.z = Mathf.Abs(_mainCamera.transform.position.z);
      currentTouchPosition = _mainCamera.ScreenToWorldPoint(currentTouchPosition);

      _inputForce.x = Mathf.InverseLerp(_settings.SafeInputDistanceX, _settings.MaxInputDistance.x, 
        Mathf.Abs(_inputService.TouchPosition.x - _touchStartPosition.x));
      _inputForce.y = Mathf.InverseLerp(-_settings.MaxInputDistance.y / 2, _settings.MaxInputDistance.y / 2, 
        _touchStartPosition.y - _inputService.TouchPosition.y);
    }

    private void UpdateAimPrediction()
    {
      float topPointX = GetAimPointPositionX();
      float targetPointX = GetAimPointPositionX(2);

      float minThrowPositionY = transform.position.y + _settings.ThrowHeight.Min;
      float topPointY = Mathf.Clamp(minThrowPositionY + _settings.ThrowHeight.Max * _inputForce.y, minThrowPositionY, float.MaxValue);

      Vector3 raycastOrigin = new Vector3(targetPointX, transform.position.y + 0.5f, transform.position.z);
      bool raycast = Physics.Raycast(raycastOrigin, -Vector3.up, out RaycastHit hit, float.MaxValue, _collideLayers);

      float aimTargetPositionY = transform.position.y;

      if (raycast)
        aimTargetPositionY = hit.point.y;
      
      _aimTopPoint.position = new Vector3(topPointX, topPointY, transform.position.z);
      _aimTargetPoint.position = new Vector3(targetPointX, aimTargetPositionY, transform.position.z);

      Vector3 rigAimTargetPosition = _aimTopPoint.position;
      rigAimTargetPosition.z = _aimPlane.position.z;
      _rigAimTarget.transform.position = rigAimTargetPosition;
    }

    private void DrawPath() 
    {
      LaunchData launchData = CalculateLaunchData();
      float timeToTarget = launchData.TimeToTarget * _settings.AimLinePercent / 100;
      _aimLine.positionCount = _settings.AimLineResolution;
      
      for (int i = 0; i <= _settings.AimLineResolution - 1; i++) 
      {
        float simulationTime = i / (float) _settings.AimLineResolution * timeToTarget;
        Vector3 displacement = launchData.InitialVelocity * simulationTime + Vector3.up * _gravity * simulationTime * simulationTime / 2f;
        Vector3 drawPoint = _aimStartPoint.position + displacement;
        _aimLine.SetPosition(i, drawPoint);
      }
    }

    private void AnimateAimLine()
    {
      float offset = Time.time * _settings.AimLineScrollSpeed;
      _aimLine.material.mainTextureOffset = new Vector2(-offset, 0);
      _aimLine.material.mainTextureScale = Vector2.Lerp(_settings.MinAimLineTilling, _settings.MaxAimLineTilling, _inputForce.x);
    }

    private LaunchData CalculateLaunchData()
    {
      float height = _aimTopPoint.position.y;
      float displacementY = _aimTargetPoint.position.y - _aimStartPoint.position.y;
      Vector3 displacementXZ = new Vector3 (_aimTargetPoint.position.x - _aimStartPoint.position.x, 0, _aimTargetPoint.position.z - _aimStartPoint.position.z);
      float time = Mathf.Sqrt(-2 * height / _gravity) + Mathf.Sqrt(2 * (displacementY - height) / _gravity);
      Vector3 velocityY = Vector3.up * Mathf.Sqrt (-2 * _gravity * height);
      Vector3 velocityXZ = displacementXZ / time;

      return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(_gravity), time);
    }
    
    private struct LaunchData 
    {
      public readonly Vector3 InitialVelocity;
      public readonly float TimeToTarget;

      public LaunchData (Vector3 initialVelocity, float timeToTarget)
      {
        InitialVelocity = initialVelocity;
        TimeToTarget = timeToTarget;
      }
    }
    
    private float GetAimPointPositionX(float multiplier = 1)
    {
      float minPositionX = transform.position.x + _settings.ThrowDistance.Min * multiplier;
      return Mathf.Clamp(minPositionX + _settings.ThrowDistance.Max * _inputForce.x * multiplier,
        minPositionX, float.MaxValue);
    }

    private void ResetAimLine() => _aimLine.positionCount = 0;
    private void ChangeAimTargetVisibility(bool visible) => _aimTargetPoint.gameObject.SetActive(visible);

    private void EnableIK()
    {
      DOTween.Kill(_disableIKTween);
      _enableIKTween = ChangeIKState(1);
    }

    private void DisableIK()
    {
      DOTween.Kill(_enableIKTween);
      _disableIKTween = ChangeIKState(0);
    }

    private void EnableIKFast()
    {
      DOTween.Kill(_enableIKTween);
      DOTween.Kill(_disableIKTween);
      _rig.weight = 1;
    }
    
    private void DisableIKFast()
    {
      DOTween.Kill(_enableIKTween);
      DOTween.Kill(_disableIKTween);
      _rig.weight = 0;
    }

    private TweenerCore<float, float, FloatOptions> ChangeIKState(float targetVaule)
    {
      float startValue = _rig.weight;
      return DOTween.To(() => startValue, x => _rig.weight = x, targetVaule, _settings.ChangeRigWeightTime);
    }
  }
}