using UnityEngine;

namespace _CodeBase.ProjectileCode.Data
{
  [CreateAssetMenu(fileName = "ProjectileSettings", menuName = "Settings/Projectile")]
  public class ProjectileSettings : ScriptableObject
  {
    public float Force;
    public float LifeTime;
    public Vector3 BloodOffset;
    [Space(10)]
    public Vector3 KnockBackForce;
    [Space(10)] 
    public LayerMask RicochetLayers;
    [Space(10)] 
    public ParticleSystem BloodVfx;
    public ParticleSystem RicochetVfx;
  }
}