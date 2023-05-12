using _CodeBase.Attributes;
using _CodeBase.HeroCode;
using _CodeBase.Infrastructure.Services;
using UnityEngine;

namespace _CodeBase.UI.Counters
{
  [AutoRegisteredService]
  public class ProjectilesCounter : CounterUI, IRegistrable
  {
    private HeroThrower _thrower;

    public void SubscribeTo(HeroThrower thrower)
    {
      _thrower = thrower;
      _thrower.ProjectilesNumberChanged += ChangeNumber;
      _thrower.TriedThrowWithoutProjectiles += Error;
    }

    public void UnSubscribe()
    {
      _thrower.ProjectilesNumberChanged -= ChangeNumber;
      _thrower.TriedThrowWithoutProjectiles -= Error;
    }
  }
}