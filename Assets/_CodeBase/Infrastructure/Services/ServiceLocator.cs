using System;
using System.Collections.Generic;
using _CodeBase.Extensions;
using UnityEngine;

namespace _CodeBase.Infrastructure.Services
{
  public class ServiceLocator
  {
    public static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();
    public static bool Initialized { get; private set; }

    public static void Clear()
    {
      Services.Clear();
      Initialized = false;
    }

    public static void Initialize()
    {
      Clear();
			
      foreach (var serviceType in ReflectionService.GetAllAutoRegisteredServices())
      {
        if (IsRegistered(serviceType)) continue;

        if (serviceType.IsMonoBehaviour())
          FindMonoService(serviceType);
        else
          RegisterNewInstance(serviceType);
      }

      Initialized = true;
    }

    public static void Register<TService>(TService service) where TService : IRegistrable, new()
    {
      if (IsRegistered<TService>()) return;

      Services[typeof(TService)] = service;
    }

    public static TService Get<TService>() where TService : IRegistrable, new()
    {
      if (Initialized == false) return new TService();
			
      var serviceType = typeof(TService);
      return (TService)Services[serviceType];
    }

    public static bool IsRegistered(Type type) => Services.ContainsKey(type);

    public static bool IsRegistered<TService>() => IsRegistered(typeof(TService));

    private static void RegisterNewInstance(Type serviceType) =>
      Services[serviceType] = Activator.CreateInstance(serviceType);

    private static object FindMonoService(Type serviceType)
    {
      var inGameService = GameObject.FindObjectOfType(serviceType);
      Services[serviceType] = inGameService;
      return inGameService;
    }
  }
}