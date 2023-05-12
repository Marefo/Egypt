using _CodeBase.Infrastructure.Services;
using UnityEngine;

namespace _CodeBase.UI.Buttons
{
  public class RestartButton : ButtonUI
  {
    [SerializeField] private SceneService _sceneService;

    protected override void OnClick() => _sceneService.ReloadCurrentScene();
  }
}