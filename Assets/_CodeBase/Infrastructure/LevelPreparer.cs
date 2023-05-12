using System;
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
    private GameState _gameState;
    
    private void Awake()
    {
      if(ServiceLocator.Initialized == false) return;
      
      _winScreen = ServiceLocator.Get<WinScreen>();
      _loseScreen = ServiceLocator.Get<LoseScreen>();
      _crystalsCounter = ServiceLocator.Get<CrystalsCounter>();
      _projectilesCounter = ServiceLocator.Get<ProjectilesCounter>();
      _gameState = ServiceLocator.Get<GameState>();

      _gameState.Reset();
      _winScreen.FastClose();
      _loseScreen.FastClose();
      _projectilesCounter.SubscribeTo(_thrower);
      _crystalsCounter.UpdateText();
    }

    private void OnDestroy()
    {
      if(ServiceLocator.Initialized == false) return;
      _projectilesCounter.UnSubscribe();
    }
  }
}