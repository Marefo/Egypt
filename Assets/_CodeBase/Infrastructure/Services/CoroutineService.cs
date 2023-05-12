using System;
using System.Collections;
using UnityEngine;

namespace _CodeBase.Infrastructure.Services
{
  public class CoroutineService : MonoBehaviour
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