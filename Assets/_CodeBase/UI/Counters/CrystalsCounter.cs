﻿using _CodeBase.Attributes;
using _CodeBase.CrystalCode.Data;
using _CodeBase.Data;
using _CodeBase.Infrastructure.Services;
using UnityEngine;

namespace _CodeBase.UI.Counters
{
  [AutoRegisteredService]
  public class CrystalsCounter : CounterUI, IRegistrable, ILevelSubscriber
  {
    [SerializeField] private CrystalsData _crystalsData;

    private void OnEnable() => _crystalsData.AmountChanged += ChangeNumber;
    private void OnDisable() => _crystalsData.AmountChanged -= ChangeNumber;

    public void UpdateText() => ChangeNumber(_crystalsData.Amount);
    
    public void OnLevelLoad() => UpdateText();

    public void OnLevelExit()
    {
    }
  }
}