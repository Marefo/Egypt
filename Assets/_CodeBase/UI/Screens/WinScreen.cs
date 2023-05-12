using System;
using _CodeBase.Infrastructure;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.UI.Screens
{
  public class WinScreen : Screen
  {
    [SerializeField] private GameState _gameState;

    private void OnEnable() => _gameState.Won += OpenWithDelay;
    private void OnDisable() => _gameState.Won -= OpenWithDelay;
  }
}