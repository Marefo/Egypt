using System;
using _CodeBase.Logic;
using UnityEngine;

namespace _CodeBase.HeroCode
{
  public class Hero : MonoBehaviour
  {
    //[SerializeField] private GameStateController _gameStateController;
    [Space(10)]
    [SerializeField] private HeroAnimator _heroAnimator;

    //private void OnEnable() => _gameStateController.Won += OnWin;
    //private void OnDisable() => _gameStateController.Won -= OnWin;

    private void OnWin() => _heroAnimator.PlayWinAnimation();
  }
}