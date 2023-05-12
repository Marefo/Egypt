using _CodeBase.CrystalCode.Data;
using UnityEngine;

namespace _CodeBase.UI.Counters
{
  public class CrystalsCounter : CounterUI
  {
    [SerializeField] private CrystalsData _crystalsData;

    private void Awake() => ChangeNumber(_crystalsData.Amount);

    private void OnEnable() => _crystalsData.AmountChanged += ChangeNumber;
    private void OnDisable() => _crystalsData.AmountChanged -= ChangeNumber;
  }
}