using System;
using System.Collections.Generic;
using System.Linq;
using _CodeBase.Attributes;
using _CodeBase.Extensions;

namespace _CodeBase.Infrastructure.Services
{
  public class ReflectionService
  {
    public static IEnumerable<Type> GetAllAutoRegisteredServices()
    {
      return AppDomain.CurrentDomain
        .GetAssemblies()
        .SelectMany(assembly => assembly.GetTypesWithCustomAttribute<AutoRegisteredService>())
        .Where(service => typeof(IRegistrable).IsAssignableFrom(service));
    }
  }
}