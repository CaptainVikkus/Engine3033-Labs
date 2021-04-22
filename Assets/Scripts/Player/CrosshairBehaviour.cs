using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairBehaviour : InputMonobehaviour
{
    public Vector2 MouseSensitivity = Vector2.one;
    public bool Inverted = false;
    public Vector2 CurrentAimPos { get; private set; }

    [SerializeField, Range(0f, 1f)]
    float CrosshairVerticalPercentage = 0.25f;
    float VerticalOffset;
    float MaxVerticalDeltaConstraint;
    float MinVerticalDeltaConstraint;

    [SerializeField, Range(0f, 1f)]
    float CrosshairHorizontalPercentage = 0.25f;
    float HorizontalOffset;
    float MaxHorizontalDeltaConstraint;
    float MinHorizontalDeltaConstraint;

    Vector2 CrosshairStartPos;
    Vector2 CurrentLookDeltas;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.CursorActive)
        {
            AppEvents.Invoke_OnMouseCursorEnable(false);
        }
        CrosshairStartPos = new Vector2(Screen.width / 2f, Screen.height / 2f);

        HorizontalOffset = (Screen.width * CrosshairHorizontalPercentage) / 2f;
        MaxHorizontalDeltaConstraint= (Screen.width / 2f) - HorizontalOffset;
        MinHorizontalDeltaConstraint = -(Screen.width /2f) + HorizontalOffset;

        VerticalOffset = (Screen.height * CrosshairVerticalPercentage) / 2f;
        MaxVerticalDeltaConstraint = (Screen.height / 2f) - VerticalOffset;
        MinVerticalDeltaConstraint = -(Screen.height / 2f) + VerticalOffset;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameInput.Player.Look.performed += OnLook;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameInput.Player.Look.performed -= OnLook;
    }
    private void OnLook(InputAction.CallbackContext delta)
    {
        Vector2 mouseDelta = delta.ReadValue<Vector2>();

        CurrentLookDeltas.x += mouseDelta.x * MouseSensitivity.x;
        if (CurrentLookDeltas.x >= MaxHorizontalDeltaConstraint || CurrentLookDeltas.x <= MinHorizontalDeltaConstraint)
            CurrentLookDeltas.x -= mouseDelta.x * MouseSensitivity.x;


        CurrentLookDeltas.y += mouseDelta.y * MouseSensitivity.y;
        if (CurrentLookDeltas.y >= MaxVerticalDeltaConstraint || CurrentLookDeltas.y <= MinVerticalDeltaConstraint)
            CurrentLookDeltas.y -= mouseDelta.y * MouseSensitivity.y;

    }

    private void Update()
    {
        float crosshairXPos = CrosshairStartPos.x + CurrentLookDeltas.x;
        float crosshairYPos = CrosshairStartPos.y + CurrentLookDeltas.y * (Inverted ? -1f: 1f);

        CurrentAimPos = new Vector2(crosshairXPos, crosshairYPos);

        transform.position = CurrentAimPos;
    }
}
