using System;
using System.Collections;
using _CodeBase.Attributes;
using UnityEngine;

namespace _CodeBase.Infrastructure.Services
{
  [AutoRegisteredService]
  public class CoroutineService : MonoBehaviour, IRegistrable
  {
    private void Awake() => StopAllCoroutines();

    public IEnumerator CallWithDelay(Action action, float delay)
    {
      yield return new WaitForSeconds(delay);
      action();
    }

    public IEnumerator CallWithDelayUnscaled(Action action, float delay)
    {
      yield return new WaitForSecondsRealtime(delay);
      action();
    }
  }
}