using System;
using _CodeBase.Logic;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.EnemyCode
{
  public class Enemy : MonoBehaviour
  {
    public event Action<Enemy> Died;
    
    public bool Alive { get; private set; } = true;

    [SerializeField] private Color _deadColor;
    [SerializeField] private float _colorChangeDuration;
    [SerializeField] private Material _deadMaterial;
    [SerializeField] private SkinnedMeshRenderer _model;
    [Space(10)]
    [SerializeField] private Collider _headCollider;
    [SerializeField] private Collider _bodyCollider;
    [SerializeField] private RagdollStateController _ragdollStateController;

    private void Start() => _ragdollStateController.Disable();

    public void OnProjectileHit(Collider hitCollider, Vector3 knockBackForce)
    {
      if(Alive == false) return;
      Die(hitCollider, knockBackForce);
    }

    private void Die(Collider hitCollider, Vector3 knockBackForce)
    {
      Alive = false;
      _model.material = _deadMaterial;
      _model.material.DOColor(_deadColor, _colorChangeDuration);

      _ragdollStateController.Enable();

      if (hitCollider == _headCollider)
      {
        knockBackForce.x /= 3;
        _ragdollStateController.AddForceToHead(knockBackForce);
      }
      else
        _ragdollStateController.AddForceToBody(knockBackForce);

      Died?.Invoke(this);
    }
  }
}