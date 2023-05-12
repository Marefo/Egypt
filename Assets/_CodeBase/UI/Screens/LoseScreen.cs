﻿using _CodeBase.Infrastructure;
using UnityEngine;

namespace _CodeBase.UI.Screens
{
  public class LoseScreen : Screen
  {
    [SerializeField] private GameState _gameState;

    private void OnEnable() => _gameState.Lost += OpenWithDelay;
    private void OnDisable() => _gameState.Lost -= OpenWithDelay;
  }
}