using System;
using _CodeBase.Attributes;
using _CodeBase.Infrastructure.Services;
using UnityEngine;

namespace _CodeBase.Infrastructure
{
  [AutoRegisteredService]
  public class GameState : MonoBehaviour, IRegistrable
  {
    public event Action GameFinished;
    public event Action Won;
    public event Action Lost;

    public bool IsGameFinished { get; private set; }

    public void Reset()
    {
      IsGameFinished = false;
    }
    
    public void Win()
    {
      if (IsGameFinished) return;
      FinishGame();
      Won?.Invoke();
    }

    public void Lose()
    {
      if (IsGameFinished) return;
      FinishGame();
      Lost?.Invoke();
    }

    private void FinishGame()
    {
      IsGameFinished = true;
      GameFinished?.Invoke();
    }
  }
}