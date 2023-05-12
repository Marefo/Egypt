using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace _CodeBase.Logic
{
  public class RagdollStateController : MonoBehaviour
  {
    [SerializeField] private Transform _ragdollParent;
    [Space(10)]
    [SerializeField] private Rigidbody _body;
    [SerializeField] private Rigidbody _head;
    [Space(10)]
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbodyForDisable;
    [SerializeField] private List<Collider> _collidersForDisable;

    private List<Rigidbody> _bodies;
    
    private void Awake() => _bodies = _ragdollParent.GetComponentsInChildren<Rigidbody>().ToList();

    public void Enable()
    {
      _animator.enabled = false;
      _rigidbodyForDisable.isKinematic = true;
      _collidersForDisable.ForEach(colliderForDisable => colliderForDisable.enabled = false);
      
      ChangeState(true);
    }

    public void Disable() => ChangeState(false);

    public void AddExplosionForce(float force, Vector3 position, float radius)
    {
      _bodies.ForEach(body => body.AddExplosionForce(force, position, radius));
    }
    
    public void AddForceToAllParts(Vector3 knockBackForce) => 
      _bodies.ForEach(body => body.AddForce(knockBackForce, ForceMode.Impulse));

    public void AddForceToHead(Vector3 knockBackForce) => 
      _head.AddForce(knockBackForce, ForceMode.Impulse);
    
    public void AddForceToBody(Vector3 knockBackForce) => 
      _body.AddForce(knockBackForce, ForceMode.Impulse);
    
    public void SetVelocityToAllParts(Vector3 velocity) => 
      _bodies.ForEach(body => body.velocity = velocity);

    private void ChangeState(bool enable) => 
      _bodies.ForEach(body => body.isKinematic = !enable);
  }
}