using System;
using _CodeBase.Attributes;
using _CodeBase.Data;
using _CodeBase.Infrastructure.Services;
using UnityEngine;

namespace _CodeBase.Infrastructure
{
  [AutoRegisteredService]
  public class GameState : MonoBehaviour, IRegistrable, ILevelSubscriber
  {
    public event Action GameFinished;
    public event Action Won;
    public event Action Lost;
    public event Action TutorialPassed;

    public bool IsGameFinished { get; private set; }
    public bool IsTutorialPassed { get; private set; }

    [SerializeField] private InputService _inputService;

    public void OnLevelLoad()
    {
      Reset();
      IsTutorialPassed = PlayerPrefs.GetInt(SaveKeys.Tutorial) == 1;

      if(IsTutorialPassed) return;
      _inputService.TouchEntered += OnTouch;
    }

    private void OnTouch()
    {
      PlayerPrefs.SetInt(SaveKeys.Tutorial, 1);
      TutorialPassed?.Invoke();
    }

    public void OnLevelExit()
    {
      if(IsTutorialPassed) return;
      _inputService.TouchEntered -= OnTouch;
    }

    public void Reset()
    {
      IsGameFinished = false;
      IsTutorialPassed = false;
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