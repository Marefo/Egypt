using System;
using _CodeBase.CrystalCode.Data;
using _CodeBase.Logic;
using _CodeBase.ProjectileCode;
using Unity.VisualScripting;
using UnityEngine;

namespace _CodeBase.CrystalCode
{
  public class Crystal : MonoBehaviour
  {
    [SerializeField] private TriggerListener _triggerListener;
    [SerializeField] private CrystalsData _data;

    private void OnEnable() => _triggerListener.Entered += OnTriggerEnter;
    private void OnDisable() => _triggerListener.Entered -= OnTriggerEnter;

    private void OnTriggerEnter(Collider obj)
    {
      if(obj.TryGetComponent(out Projectile projectile) == false) return;
      _data.Add();
      Destroy(gameObject);
    }
  }
}