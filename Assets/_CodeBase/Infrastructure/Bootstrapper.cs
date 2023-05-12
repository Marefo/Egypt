using System;
using System.Collections.Generic;
using _CodeBase.Extensions;
using _CodeBase.Infrastructure.Services;
using UnityEngine;

namespace _CodeBase.Infrastructure
{
  public class Bootstrapper : MonoBehaviour
  {
    [SerializeField] private List<GameObject> _indestructibleObjects;
    [Space(10)]
    [SerializeField] private SceneService _sceneServicePrefab;
    
    private const string BootstrapSceneName = "Bootstrap";
    private const string FirstLevelSceneName = "Level-1";
    
    private void Awake()
    {
      if(ServiceLocator.Initialized) return;
      Initialize();
    }

    private void Initialize()
    {
      SceneService sceneService;

      if (gameObject.scene.name != BootstrapSceneName)
      {
        sceneService = Instantiate(_sceneServicePrefab);
        sceneService.LoadScene(BootstrapSceneName);
        return;
      }

      ServiceLocator.Initialize();
      _indestructibleObjects.ForEach(DontDestroyOnLoad);
      SetIndestructibleServiceObjects();

      sceneService = ServiceLocator.Get<SceneService>();
      sceneService.LoadScene(FirstLevelSceneName);
    }


    private void SetIndestructibleServiceObjects()
    {
      foreach (var service in ServiceLocator.Services)
      {
        if(service.Key.IsMonoBehaviour() == false) continue;
        MonoBehaviour serviceScript = (MonoBehaviour)service.Value;
        DontDestroyOnLoad(serviceScript.gameObject.transform.root); 
      }
    }
  }
}