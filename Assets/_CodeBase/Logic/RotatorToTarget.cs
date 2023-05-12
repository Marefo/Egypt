using DG.Tweening;
using UnityEngine;

namespace _CodeBase.Logic
{
  public class RotatorToTarget : MonoBehaviour
  {
    [SerializeField] private Transform _rotateObj;
    [SerializeField] private Transform _targetObj;
    [SerializeField] private float _rotationSpeed;
    [Space(10)] 
    [SerializeField] private float _resetRotationTime;

    private bool _enabled;
    private Vector3 _defaultRotation;

    private void Awake() => 
      _defaultRotation = _rotateObj.transform.rotation.eulerAngles;

    private void Update()
    {
      if(_enabled == false) return;

      Vector3 targetPosition = _targetObj.position;
      targetPosition.y = _rotateObj.position.y;
      Quaternion lookRotation = Quaternion.LookRotation(targetPosition - _rotateObj.position);
      _rotateObj.rotation = Quaternion.Slerp(_rotateObj.rotation, lookRotation, _rotationSpeed * Time.deltaTime);
    }

    public void Enable()
    {
      _enabled = true;
      _rotateObj.DOKill();
    }

    public void Disable() => _enabled = false;

    public void ResetRotation()
    {
      _rotateObj.DOKill();
      _rotateObj.DORotate(_defaultRotation, _resetRotationTime);
    }
  }
}