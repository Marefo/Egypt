using _CodeBase.HeroCode;
using UnityEngine;

namespace _CodeBase.UI.Counters
{
  public class ProjectilesCounter : CounterUI
  {
    [SerializeField] private HeroThrower _thrower;

    private void OnEnable()
    {
      _thrower.ProjectilesNumberChanged += ChangeNumber;
      _thrower.TriedThrowWithoutProjectiles += Error;
    }

    private void OnDisable()
    {
      _thrower.ProjectilesNumberChanged -= ChangeNumber;
      _thrower.TriedThrowWithoutProjectiles -= Error;
    }
  }
}