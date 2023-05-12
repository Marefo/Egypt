using System;
using System.Collections;
using _CodeBase.Extensions;
using _CodeBase.HeroCode;
using UnityEngine;
using Range = _CodeBase.Data.Range;

namespace _CodeBase.CameraCode
{
  public class AimCameraZoomer : MonoBehaviour
  {
    [SerializeField] private Range _zoom;
    [SerializeField] private float _speed;
    [Space(10)]
    [SerializeField] private HeroAimer _aimer;
    [SerializeField] private Transform _cameraTarget;

    private Coroutine _zoomCoroutine;
    private bool _aiming;

    private void OnEnable()
    {
      _aimer.AimStarted += OnAimStart;
      _aimer.AimFinished += OnAimFinish;
    }
    
    private void OnDisable()
    {
      _aimer.AimStarted -= OnAimStart;
      _aimer.AimFinished -= OnAimFinish;
    }

    private void Start() => StartCoroutine(ZoomCoroutine());

    private void OnAimStart() => _aiming = true;
    private void OnAimFinish() => _aiming = false;
    
    private IEnumerator ZoomCoroutine()
    {
      while (true)
      {
        float targetZoomValue = _aiming ? Mathf.Lerp(_zoom.Max, _zoom.Min, _aimer.InputForce.x) : _zoom.Max;
        
        if (Math.Abs(_cameraTarget.position.z - targetZoomValue) > 0.2f)
        {
          float currentZoomValue = _cameraTarget.position.z + _cameraTarget.position.z.GetSignForInterpolation(targetZoomValue) * _speed * Time.unscaledDeltaTime;
          currentZoomValue = Mathf.Clamp(currentZoomValue, _zoom.Min, _zoom.Max);
          _cameraTarget.position = new Vector3(_cameraTarget.position.x, _cameraTarget.position.y, currentZoomValue);
        }
        
        yield return null;
      }
    }
  }
}