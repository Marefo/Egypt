using System;
using _CodeBase.Infrastructure;
using _CodeBase.Infrastructure.Services;
using UnityEngine;

namespace _CodeBase.HeroCode
{
  public class Hero : MonoBehaviour
  {
    [SerializeField] private HeroAnimator _heroAnimator;
    
    private GameState _gameState;

    private void Awake() => _gameState = ServiceLocator.Get<GameState>();

    private void OnEnable() => _gameState.Won += OnWin;
    private void OnDisable() => _gameState.Won -= OnWin;

    private void OnWin() => _heroAnimator.PlayWinAnimation();
  }
}