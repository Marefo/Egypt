using _CodeBase.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _CodeBase.UI
{
  public abstract class CounterUI : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _textField;
    [Space(10)]
    [SerializeField] private PunchScaleSettings _punchScaleSettings;

    protected void Error()
    {
      _textField.color = Color.white;
      _textField.DOColor(Color.red, _punchScaleSettings.Duration)
        .OnKill(() => _textField.color = Color.white).SetLink(_textField.gameObject);
      _textField.transform.DOPunchScale(Vector3.one * _punchScaleSettings.Punch, _punchScaleSettings.Duration,
        _punchScaleSettings.Vibrato, _punchScaleSettings.Elasticity).SetLink(_textField.gameObject);
    }
    
    protected void ChangeNumber(int newNumber)
    {
      _textField.text = newNumber.ToString();
      _textField.transform.DOPunchScale(Vector3.one * _punchScaleSettings.Punch, _punchScaleSettings.Duration,
        _punchScaleSettings.Vibrato, _punchScaleSettings.Elasticity).SetLink(_textField.gameObject);
    }
  }
}