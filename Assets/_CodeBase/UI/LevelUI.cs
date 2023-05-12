using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _CodeBase.UI
{
  public class LevelUI : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _textField;
    
    private void Start() => _textField.text = SceneManager.GetActiveScene().name;
  }
}