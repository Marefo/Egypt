using _CodeBase.Infrastructure.Services;
using UnityEngine;

namespace _CodeBase.UI.Buttons
{
  public class NextLevelButton : ButtonUI
  {
    [SerializeField] private SceneService _sceneService;

    protected override void OnClick() => _sceneService.LoadNextScene();
  }
}