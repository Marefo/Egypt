using _CodeBase.Attributes;
using _CodeBase.Infrastructure;
using _CodeBase.Infrastructure.Services;
using UnityEngine;

namespace _CodeBase.UI.Screens
{
  [AutoRegisteredService]
  public class LoseScreen : Screen, IRegistrable
  {
    [SerializeField] private GameState _gameState;

    private void OnEnable() => _gameState.Lost += OpenWithDelay;
    private void OnDisable() => _gameState.Lost -= OpenWithDelay;
  }
}