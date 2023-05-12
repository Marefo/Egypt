using System;
using _CodeBase.Attributes;
using _CodeBase.Data;
using _CodeBase.Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _CodeBase.UI
{
  [AutoRegisteredService]
  public class LevelUI : MonoBehaviour, IRegistrable, ILevelSubscriber
  {
    [SerializeField] private TextMeshProUGUI _textField;
    
    public void OnLevelLoad()
    {
      _textField.text = SceneManager.GetActiveScene().name;
    }

    public void OnLevelExit()
    {
    }
  }
}