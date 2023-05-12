﻿using _CodeBase.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _CodeBase.Infrastructure.Services
{
  [AutoRegisteredService]
  public class SceneService : MonoBehaviour, IRegistrable
  {
    public void ReloadCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void LoadNextScene()
    {
      int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

      if (nextSceneIndex > SceneManager.sceneCountInBuildSettings - 1)
        nextSceneIndex = 1;
        
      SceneManager.LoadScene(nextSceneIndex);
    }

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
  }
}