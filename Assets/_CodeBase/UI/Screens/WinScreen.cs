using System;
using _CodeBase.Attributes;
using _CodeBase.Data;
using _CodeBase.Infrastructure;
using _CodeBase.Infrastructure.Services;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.UI.Screens
{
  [AutoRegisteredService]
  public class WinScreen : Screen, IRegistrable
  {
    [SerializeField] private GameState _gameState;

    private void OnEnable() => _gameState.Won += OpenWithDelay;
    private void OnDisable() => _gameState.Won -= OpenWithDelay;
  }
}