using System;
using UnityEngine;

namespace _CodeBase.Infrastructure
{
  public class GameState : MonoBehaviour
  {
    public event Action GameFinished;
    public event Action Won;
    public event Action Lost;
    
    public bool IsGameFinished { get; private set; }
    
    public void Win()
    {
      if(IsGameFinished) return;
      FinishGame();
      Won?.Invoke();
    }

    public void Lose()
    {
      if(IsGameFinished) return;
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