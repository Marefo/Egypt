using System;
using UnityEngine;

namespace _CodeBase.CrystalCode.Data
{
  [CreateAssetMenu(fileName = "CrystalsData", menuName = "StaticData/Crystals")]
  public class CrystalsData : ScriptableObject
  {
    public event Action<int> AmountChanged;
    
    public int Amount { get; private set; }

    public void Add()
    {
      Amount += 1;
      AmountChanged?.Invoke(Amount);
    }
  }
}