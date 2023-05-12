using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using SystemRandom = System.Random;

namespace _CodeBase.Extensions
{
  public static class Extensions
  {
    private static readonly SystemRandom rng = new SystemRandom(); 
		
    public static bool IsMonoBehaviour(this Type type) => typeof(MonoBehaviour).IsAssignableFrom(type);
    
    public static IEnumerable<Type> GetTypesWithCustomAttribute<TAttribute>(this Assembly assembly) where TAttribute : Attribute
    {
      return assembly.GetTypes().Where(t => t.GetCustomAttribute<TAttribute>() != null);
    }
    
    public static T GetRandomValue<T>(this List<T> list) => list[Random.Range(0, list.Count)];
		
    public static List<T> Shuffle<T>(this List<T> list)
    {
      List<T> result = new List<T>(list);
      int n = result.Count;  
      while (n > 1) {  
        n--;  
        int k = rng.Next(n + 1);
        (result[k], result[n]) = (result[n], result[k]);
      }
      return result;
    }
		
    public static bool AddIfNotExists<T>(this List<T> list, T value)
    {
      if (list.Contains(value)) return false;
			
      list.Add(value);
      return true;
    }
    
    public static bool CompareLayers(this GameObject obj, LayerMask layerMask) => 
      layerMask == (layerMask | (1 << obj.layer));

    public static void ChangeImageAlpha(this Image image, float targetAlpha)
    {
      Color imageColor = image.color;
      imageColor.a = targetAlpha;
      image.color = imageColor;
    }
    
    public static int GetSignForInterpolation(this float currentValue, float targetValue) => 
      targetValue > currentValue ? 1 : -1;
  }
}