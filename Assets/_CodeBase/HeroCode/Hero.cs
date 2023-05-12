using _CodeBase.Infrastructure;
using UnityEngine;

namespace _CodeBase.HeroCode
{
  public class Hero : MonoBehaviour
  {
    [SerializeField] private GameState _gameState;
    [Space(10)]
    [SerializeField] private HeroAnimator _heroAnimator;

    private void OnEnable() => _gameState.Won += OnWin;
    private void OnDisable() => _gameState.Won -= OnWin;

    private void OnWin() => _heroAnimator.PlayWinAnimation();
  }
}