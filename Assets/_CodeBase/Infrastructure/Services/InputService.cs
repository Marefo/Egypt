﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _CodeBase.Infrastructure.Services
{
  public class InputService : MonoBehaviour
  {
    public event Action Disabled;
    public event Action TouchEntered;
    public event Action TouchCanceled;
		
    public bool Enabled { get; private set; } = true;
    public Vector2 TouchPosition { get; private set; }
    public Vector2 TouchStartPosition { get; private set; }

    private InputActions _inputActions;
    private bool _touchEntered;
    
    private void Awake() => _inputActions = new InputActions();

    private void OnEnable()
    {
      _inputActions.Enable();
      _inputActions.Game.TouchPress.started += OnTouchEnter;
      _inputActions.Game.TouchPress.canceled += OnTouchCancel;
    }

    private void OnDisable()
    {
      _inputActions.Disable();
      _inputActions.Game.TouchPress.started -= OnTouchEnter;
      _inputActions.Game.TouchPress.canceled -= OnTouchCancel;
    }

    private void Update()
    {
      if(Enabled == false) return;
      UpdateTouchPosition();
    }

    private void OnTouchEnter(InputAction.CallbackContext ctx)
    {
      if(Enabled == false || Helpers.IsPointerOverUIObject()) return;
      _touchEntered = true;
      TouchStartPosition = TouchPosition;
      TouchEntered?.Invoke();
    }
    

    private void OnTouchCancel(InputAction.CallbackContext obj)
    {
      if(Enabled == false || _touchEntered == false) return;
      _touchEntered = false;
      TouchCanceled?.Invoke();
    }

    private void UpdateTouchPosition() => 
      TouchPosition = _inputActions.Game.TouchPosition.ReadValue<Vector2>();

    public void Enable() => Enabled = true;
    public void Disable()
    {
      Enabled = false;
      Disabled?.Invoke();
    }
  }
}