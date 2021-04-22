using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMonobehaviour : MonoBehaviour
{

    protected PlayerInputActions GameInput;

    protected virtual void Awake()
    {
        GameInput = new PlayerInputActions();
    }

    protected virtual void OnEnable()
    {
        GameInput.Enable();
    }

    protected virtual void OnDisable()
    {
        GameInput.Disable();
    }
}
