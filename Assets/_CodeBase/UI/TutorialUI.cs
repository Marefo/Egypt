using System;
using _CodeBase.Data;
using _CodeBase.Infrastructure;
using _CodeBase.Infrastructure.Services;
using UnityEngine;

namespace _CodeBase.UI
{
  public class TutorialUI : MonoBehaviour
  {
    [SerializeField] private GameState _gameState;

    private void OnEnable() => _gameState.TutorialPassed += Hide;
    private void OnDisable() => _gameState.TutorialPassed -= Hide;

    private void Start()
    {
      if(_gameState.IsTutorialPassed)
        Hide();
    }

    private void Hide() => gameObject.SetActive(false);
  }
}