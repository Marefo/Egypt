using _CodeBase.Data;
using UnityEngine;

namespace _CodeBase.HeroCode.Data
{
  [CreateAssetMenu(fileName = "HeroAimSettings", menuName = "Settings/Hero/Aim")]
  public class HeroAimSettings : ScriptableObject
  {
    public float SafeInputDistanceX;
    public Vector2 MaxInputDistance;
    [Space(10)] 
    public Range ThrowDistance;
    public Range ThrowHeight;
    [Space(10)] 
    public Vector2 MinAimLineTilling;
    public Vector2 MaxAimLineTilling;
    [Space(10)]
    public int AimLineResolution;
    public float AimLineScrollSpeed;
    [Range(0, 100)] public int AimLinePercent; 
    [Space(10)]
    public float ResetAimDelay;
    public float ChangeRigWeightTime;
  }
}