using UnityEngine;

namespace _CodeBase.HeroCode.Data
{
  [CreateAssetMenu(fileName = "HeroThrower", menuName = "Settings/Hero/Thrower")]
  public class HeroThrowerSettings : ScriptableObject
  {
    public int ProjectilesNumber;
  }
}