﻿using _CodeBase.UI.Screens.Data;
using DG.Tweening;
using UnityEngine;

namespace _CodeBase.UI.Screens
{
  public class Screen : MonoBehaviour
  {
    public bool IsVisible => _visual.localScale != Vector3.zero;
    
    [SerializeField] private Transform _visual;
    [Space(10)]
    [SerializeField] private ScreenSettings _settings;

    private Tween _openWithDelayTween;
    
    public virtual void OpenWithDelay()
    {
      _openWithDelayTween?.Kill();
      _openWithDelayTween = DOVirtual.DelayedCall(_settings.ShowDelay, Open).SetLink(gameObject);
    }
    
    public virtual void Open()
    {
      _visual.DOKill();
      _visual.DOScale(Vector3.one, 0.15f)
        .SetUpdate(true)
        .SetLink(gameObject);
    }

    public virtual void Close()
    {
      _visual.DOKill();
      _visual.DOScale(Vector3.zero, 0.15f)
        .SetUpdate(true)
        .SetLink(gameObject);
    }
  }
}