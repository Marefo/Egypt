using System;
using System.Collections.Generic;
using _CodeBase.Data;
using _CodeBase.HeroCode;
using _CodeBase.Infrastructure.Services;
using _CodeBase.ProjectileCode;
using _CodeBase.UI.Counters;
using _CodeBase.UI.Screens;
using UnityEngine;

namespace _CodeBase.Infrastructure
{
  public class LevelPreparer : MonoBehaviour
  {
    [SerializeField] private HeroThrower _thrower;
    
    private WinScreen _winScreen;
    private LoseScreen _loseScreen;
    private CrystalsCounter _crystalsCounter;
    private ProjectilesCounter _projectilesCounter;
    private List<ILevelSubscriber> _levelSubscribers = new List<ILevelSubscriber>();

    private void Awake()
    {
      if(ServiceLocator.Initialized == false) return;
      
      _projectilesCounter = ServiceLocator.Get<ProjectilesCounter>();

      foreach (KeyValuePair<Type,object> pair in ServiceLocator.Services)
      {
        if (pair.Value is ILevelSubscriber)
        {
          ILevelSubscriber subscriber = (ILevelSubscriber) pair.Value;
          _levelSubscribers.Add(subscriber);
          subscriber.OnLevelLoad();
        }
      }
      
      _projectilesCounter.SubscribeTo(_thrower);
    }

    private void OnDestroy()
    {
      if(ServiceLocator.Initialized == false) return;
      _projectilesCounter.UnSubscribe();
      _levelSubscribers.ForEach(subscriber => subscriber.OnLevelExit());
    }
  }
}